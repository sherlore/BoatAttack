using UnityEngine;
using UnityEngine.InputSystem;

namespace BoatAttack
{
    /// <summary>
    /// This sends input controls to the boat engine if 'Human'
    /// </summary>
    public class HumanController : BaseController
    {
        private InputControls _controls;

        public float _throttle;
        public float _steering;

        private bool _paused;
					
		public RabboniModule leftModule;
		public RabboniModule rightModule;
        
        private void Awake()
        {
            _controls = new InputControls();
            
            /*_controls.BoatControls.Trottle.performed += context => _throttle = context.ReadValue<float>();
            _controls.BoatControls.Trottle.canceled += context => _throttle = 0f;
            
            _controls.BoatControls.Steering.performed += context => _steering = context.ReadValue<float>();
            _controls.BoatControls.Steering.canceled += context => _steering = 0f;*/

            _controls.BoatControls.Reset.performed += ResetBoat;
            _controls.BoatControls.Pause.performed += FreezeBoat;

            _controls.DebugControls.TimeOfDay.performed += SelectTime;
        }
		
		void Start()
		{
			leftModule = RabboniConsole.instance.listDic["RabboniLeft"];
			rightModule = RabboniConsole.instance.listDic["RabboniRight"];
		}

        public override void OnEnable()
        {
            base.OnEnable();
            _controls.BoatControls.Enable();
        }

        private void OnDisable()
        {
            _controls.BoatControls.Disable();
        }

        private void ResetBoat(InputAction.CallbackContext context)
        {
            controller.ResetPosition();
        }

        private void FreezeBoat(InputAction.CallbackContext context)
        {
            _paused = !_paused;
            if(_paused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        private void SelectTime(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            Debug.Log($"changing day time, input:{value}");
            DayNightController.SelectPreset(value);
        }

        void FixedUpdate()
        {
			float leftPower = Mathf.Lerp(0f, 1f, (leftModule.lastAcc.magnitude-1)/3f);
			float rightPower = Mathf.Lerp(0f, 1f, (rightModule.lastAcc.magnitude-1)/3f);
			
			_throttle = (leftPower+rightPower)*0.5f;
			
			_steering = leftPower - rightPower;
			
            engine.Accelerate(_throttle);
            engine.Turn(_steering);
        }
    }
}

