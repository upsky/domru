using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shapes
{
    public abstract class Shape : MonoBehaviour
    {
        public readonly bool[] Sides = new bool[4];

        [HideInInspector]
        public int Xindex;

        [HideInInspector]
        public int Yindex;

        /// <remarks>константа, т.к. скорость общая для всех фигур</remarks>
        private const float _rotationSpeed = 200f;

        [SerializeField]//, HideInInspector]
        protected Direction _currentDirection = _defaultStartDirection;

        private const Direction _defaultStartDirection = Direction.Up;

        protected bool _isInRotateProcess;
        private float _rotationRemain;
        
        /// <summary>
        /// Положение, которое займет объект, после прекращения вращения на 90 градусов
        /// </summary>
        private float _targetRotationAngle;
        //public Direction CorrectDirection;

        public bool Up
        {
            get { return Sides[0]; }
            protected set { Sides[0] = value; }
        }

        public bool Right
        {
            get { return Sides[1]; }
            protected set { Sides[1] = value; }
        }

        public bool Down
        {
            get { return Sides[2]; }
            protected set { Sides[2] = value; }
        }

        public bool Left
        {
            get { return Sides[3]; }
            protected set { Sides[3] = value; }
        }

        protected virtual void Awake()
        {
            //автоустановка правильного значения _currentDirection при старте игры
            _currentDirection = DirectionUtils.EulerAngleToDirection(transform.rotation.eulerAngles.y);
        }

        private void Update()
        {
            if (_isInRotateProcess)
            {
                var rotationDelta = _rotationSpeed * Time.deltaTime;
                _rotationRemain -= rotationDelta;
                if (_rotationRemain >= 0)
                {
                    transform.Rotate(0, -(rotationDelta), 0);
                }
                else
                {
                    transform.SetRotationEulerY(_targetRotationAngle);
                    _isInRotateProcess = false;
                }
            }
        }

        private void OnClick()
        {
            //Debug.LogWarning("shape");
            RotateToLeft();
            ConnectorsManager.CheckAllConnections();
            //CheckAllConnections - временно здесь, пока не реализованы сигналы 
        }

        public void RotateToDirection(Direction direction)
        {
            StartCoroutine(RotateToDirectionCoroutine(direction));
        }

        //FastRotate() - пригодится, когда нужно будет делать генерацию.
        //{UpdateInnerRotateVariables();
        //transform.Rotate(0, -90, 0);
        //}

        public void RotateToLeft()
        {
            if (_isInRotateProcess)
                return;
            UpdateInnerRotateVariables();
            _rotationRemain = 90;
            _targetRotationAngle = transform.rotation.eulerAngles.y - 90;
            _isInRotateProcess = true;
        }

        private IEnumerator RotateToDirectionCoroutine(Direction direction)
        {
            if (_isInRotateProcess)
                yield break;
            while (CanContinueRotating(direction))
            {
                if (!_isInRotateProcess)
                    RotateToLeft();
                yield return new WaitForSeconds(0.02f);
            }
        }

        /// <summary>
        /// Условие продолжения выполнения корутины RotateToDirectionCoroutine
        /// </summary>
        protected virtual bool CanContinueRotating(Direction targetDirection)
        {
            return _currentDirection != targetDirection;
        }
        

        private void UpdateInnerRotateVariables()
        {
            _currentDirection = _currentDirection.GetPrev();
            bool tempSide = Sides[0];
            for (int i = 0; i < 3; i++)
                Sides[i] = Sides[i + 1];
            Sides[3] = tempSide;
        }


        #if UNITY_EDITOR
        public void RotateThroughInspector()
        {
            UpdateInnerRotateVariables();
            transform.Rotate(0, -90, 0);
            _currentDirection = DirectionUtils.EulerAngleToDirection(transform.rotation.eulerAngles.y);
        }

        /// <summary>
        /// Используется для корретировки значения в режиме редактора. В коде игры не использовать 
        /// </summary>
        public void UpdateCurrentDirectionInEditorMode()
        {            
            if (!Application.isPlaying)
                _currentDirection = DirectionUtils.EulerAngleToDirection(transform.rotation.eulerAngles.y);
        }
        #endif

        /// <summary>
        /// Проверка наличия соединения
        /// </summary>
        /// <param name="prevOutDirection">Направление выхода предыдущего элемента. Противоположно проверяемой стороне текущего элемента</param>
        public bool HasConnection(Direction prevOutDirection)
        {
            return Sides[(byte)prevOutDirection.GetOpposite()];
        }

        //public bool HasConnection(Shape prevShape)
        //{
        //    return Sides[(byte)_currentDirection.GetOpposite()] && Sides[(byte)prevShape._currentDirection];
        //}

        /// <summary>
        /// Возвращает выходное направление, относительно входного направления.
        /// </summary>
        /// <param name="prevOutDirection">Направление выхода предыдущего элемента. Противоположно проверяемой стороне текущего элемента</param>
        public abstract Direction GetOutDirection(Direction prevOutDirection);

        /// <summary>
        /// Вовзращает точки пути для сигнала, учитывая с какой стороны пришел сигнал
        /// </summary>
        /// <param name="prevOutDirection">Направление выхода предыдущего элемента. Противоположно проверяемой стороне текущего элемента</param>
        public abstract List<Vector3> GetPath(Direction prevOutDirection);

        public abstract List<string> TestPath(Direction prevOutDirection);
    }

}