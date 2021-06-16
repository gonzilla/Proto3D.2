// GENERATED AUTOMATICALLY FROM 'Assets/Script/PlayersControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @NewGestionInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @NewGestionInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayersControls"",
    ""maps"": [
        {
            ""name"": ""Manette"",
            ""id"": ""87dc6f22-daa2-4836-b8be-8313e1a06452"",
            ""actions"": [
                {
                    ""name"": ""Avancer"",
                    ""type"": ""Value"",
                    ""id"": ""5dd11aec-90bb-4334-b8e5-bd5f64bbf6e1"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Derapage"",
                    ""type"": ""Button"",
                    ""id"": ""285ae062-193b-4111-95fd-f0f83eb50cf2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Tourner"",
                    ""type"": ""Button"",
                    ""id"": ""acfbcbb4-0cf8-45b7-9d54-cb65517e3dfa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Straffer"",
                    ""type"": ""Button"",
                    ""id"": ""671ed660-313c-4ab7-ba72-43e8e2ec959d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""f880c134-2545-440c-8152-3de2f7cd3e68"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ResetPosition"",
                    ""type"": ""Button"",
                    ""id"": ""6ff71aa2-c27f-4043-a03d-92ef5ef37863"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ResetScene"",
                    ""type"": ""Button"",
                    ""id"": ""a234272d-b718-41c0-b3ab-5bc16b2d3b1b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Moteur"",
                    ""id"": ""5093f7aa-784b-4a3f-8d7d-95379f44902c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Avancer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f24e1e96-1ab9-4380-8450-871571898186"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Avancer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""193a1b33-4cfc-45ba-b178-9e5c020cd746"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Avancer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""DirectionDerap"",
                    ""id"": ""d0b3a578-9a91-44a5-aad9-94f950216659"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Derapage"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c81794e8-fc57-45fc-bbc3-34d625394ab3"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Derapage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a980fb05-4b9f-4c68-b806-fea9aac6f66b"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Derapage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""tourne"",
                    ""id"": ""274ac0f9-d657-4e2d-bb1a-781370620119"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tourner"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""9acd9de1-a0b2-44c4-a5fc-5c4879374e22"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tourner"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""95d01bf4-2017-4907-ad57-879f89d39b03"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tourner"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""straf"",
                    ""id"": ""02fad3ba-4dd2-40d3-9a50-f234b9c0597b"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Straffer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""20d732bc-32e8-4617-9e35-063b8b094522"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Straffer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8696b864-a8dd-44aa-a07d-3d9465667fcf"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Straffer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3752b5a7-64e6-4ceb-8865-6e5b00b23736"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResetPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df9d6a47-af6a-4879-ba5d-9c9ab9968c0b"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResetScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35c31ac9-1fc3-4762-b456-4cba906a276d"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Manette
        m_Manette = asset.FindActionMap("Manette", throwIfNotFound: true);
        m_Manette_Avancer = m_Manette.FindAction("Avancer", throwIfNotFound: true);
        m_Manette_Derapage = m_Manette.FindAction("Derapage", throwIfNotFound: true);
        m_Manette_Tourner = m_Manette.FindAction("Tourner", throwIfNotFound: true);
        m_Manette_Straffer = m_Manette.FindAction("Straffer", throwIfNotFound: true);
        m_Manette_Boost = m_Manette.FindAction("Boost", throwIfNotFound: true);
        m_Manette_ResetPosition = m_Manette.FindAction("ResetPosition", throwIfNotFound: true);
        m_Manette_ResetScene = m_Manette.FindAction("ResetScene", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Manette
    private readonly InputActionMap m_Manette;
    private IManetteActions m_ManetteActionsCallbackInterface;
    private readonly InputAction m_Manette_Avancer;
    private readonly InputAction m_Manette_Derapage;
    private readonly InputAction m_Manette_Tourner;
    private readonly InputAction m_Manette_Straffer;
    private readonly InputAction m_Manette_Boost;
    private readonly InputAction m_Manette_ResetPosition;
    private readonly InputAction m_Manette_ResetScene;
    public struct ManetteActions
    {
        private @NewGestionInput m_Wrapper;
        public ManetteActions(@NewGestionInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Avancer => m_Wrapper.m_Manette_Avancer;
        public InputAction @Derapage => m_Wrapper.m_Manette_Derapage;
        public InputAction @Tourner => m_Wrapper.m_Manette_Tourner;
        public InputAction @Straffer => m_Wrapper.m_Manette_Straffer;
        public InputAction @Boost => m_Wrapper.m_Manette_Boost;
        public InputAction @ResetPosition => m_Wrapper.m_Manette_ResetPosition;
        public InputAction @ResetScene => m_Wrapper.m_Manette_ResetScene;
        public InputActionMap Get() { return m_Wrapper.m_Manette; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ManetteActions set) { return set.Get(); }
        public void SetCallbacks(IManetteActions instance)
        {
            if (m_Wrapper.m_ManetteActionsCallbackInterface != null)
            {
                @Avancer.started -= m_Wrapper.m_ManetteActionsCallbackInterface.OnAvancer;
                @Avancer.performed -= m_Wrapper.m_ManetteActionsCallbackInterface.OnAvancer;
                @Avancer.canceled -= m_Wrapper.m_ManetteActionsCallbackInterface.OnAvancer;
                @Derapage.started -= m_Wrapper.m_ManetteActionsCallbackInterface.OnDerapage;
                @Derapage.performed -= m_Wrapper.m_ManetteActionsCallbackInterface.OnDerapage;
                @Derapage.canceled -= m_Wrapper.m_ManetteActionsCallbackInterface.OnDerapage;
                @Tourner.started -= m_Wrapper.m_ManetteActionsCallbackInterface.OnTourner;
                @Tourner.performed -= m_Wrapper.m_ManetteActionsCallbackInterface.OnTourner;
                @Tourner.canceled -= m_Wrapper.m_ManetteActionsCallbackInterface.OnTourner;
                @Straffer.started -= m_Wrapper.m_ManetteActionsCallbackInterface.OnStraffer;
                @Straffer.performed -= m_Wrapper.m_ManetteActionsCallbackInterface.OnStraffer;
                @Straffer.canceled -= m_Wrapper.m_ManetteActionsCallbackInterface.OnStraffer;
                @Boost.started -= m_Wrapper.m_ManetteActionsCallbackInterface.OnBoost;
                @Boost.performed -= m_Wrapper.m_ManetteActionsCallbackInterface.OnBoost;
                @Boost.canceled -= m_Wrapper.m_ManetteActionsCallbackInterface.OnBoost;
                @ResetPosition.started -= m_Wrapper.m_ManetteActionsCallbackInterface.OnResetPosition;
                @ResetPosition.performed -= m_Wrapper.m_ManetteActionsCallbackInterface.OnResetPosition;
                @ResetPosition.canceled -= m_Wrapper.m_ManetteActionsCallbackInterface.OnResetPosition;
                @ResetScene.started -= m_Wrapper.m_ManetteActionsCallbackInterface.OnResetScene;
                @ResetScene.performed -= m_Wrapper.m_ManetteActionsCallbackInterface.OnResetScene;
                @ResetScene.canceled -= m_Wrapper.m_ManetteActionsCallbackInterface.OnResetScene;
            }
            m_Wrapper.m_ManetteActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Avancer.started += instance.OnAvancer;
                @Avancer.performed += instance.OnAvancer;
                @Avancer.canceled += instance.OnAvancer;
                @Derapage.started += instance.OnDerapage;
                @Derapage.performed += instance.OnDerapage;
                @Derapage.canceled += instance.OnDerapage;
                @Tourner.started += instance.OnTourner;
                @Tourner.performed += instance.OnTourner;
                @Tourner.canceled += instance.OnTourner;
                @Straffer.started += instance.OnStraffer;
                @Straffer.performed += instance.OnStraffer;
                @Straffer.canceled += instance.OnStraffer;
                @Boost.started += instance.OnBoost;
                @Boost.performed += instance.OnBoost;
                @Boost.canceled += instance.OnBoost;
                @ResetPosition.started += instance.OnResetPosition;
                @ResetPosition.performed += instance.OnResetPosition;
                @ResetPosition.canceled += instance.OnResetPosition;
                @ResetScene.started += instance.OnResetScene;
                @ResetScene.performed += instance.OnResetScene;
                @ResetScene.canceled += instance.OnResetScene;
            }
        }
    }
    public ManetteActions @Manette => new ManetteActions(this);
    public interface IManetteActions
    {
        void OnAvancer(InputAction.CallbackContext context);
        void OnDerapage(InputAction.CallbackContext context);
        void OnTourner(InputAction.CallbackContext context);
        void OnStraffer(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
        void OnResetPosition(InputAction.CallbackContext context);
        void OnResetScene(InputAction.CallbackContext context);
    }
}
