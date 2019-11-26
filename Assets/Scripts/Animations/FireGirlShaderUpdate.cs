using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGirlShaderUpdate : MonoBehaviour
{
    // Start is called before the first frame update

    public FireGirlShaderUpdateType ComponentType;
    private MovementController _movementController;
    private Material[] _materials;

    public enum FireGirlShaderUpdateType
    {
        Body,
        Hair,
        TransparentBody,
        TransparentHair,
        Jewelry,
    }

    public FloatVariable Health;
    //public float BodyMaxDissolveAmount;
    public float TransparentVelocityMultiplier = 1f;



    void Start()
    {
        _movementController = FindObjectOfType<MovementController>();
        _materials = GetComponent<Renderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        switch (ComponentType)
        {
            case FireGirlShaderUpdateType.Body:
                foreach (Material material in _materials)
                    material.SetFloat("_DissolveAmount", 1f - Health.Value / 100f);
                break;

            case FireGirlShaderUpdateType.Hair:
                float glow = (Health.Value > 50f) ? 1f : Health.Value / 50f;
                foreach (Material material in _materials)
                    material.SetFloat("_GlowAmount", glow);
                break;

            case FireGirlShaderUpdateType.TransparentBody:
            case FireGirlShaderUpdateType.TransparentHair:
                Vector3 flameDirection = Vector3.up;
                if (_movementController.IsMoving)
                    flameDirection -= _movementController.DashDirection() * TransparentVelocityMultiplier;
                foreach (Material material in _materials)
                    material.SetVector("_FlameDirection", flameDirection);
                break;

            case FireGirlShaderUpdateType.Jewelry:
                // In case we modify the jewelry.
                break;

            default:
                throw new System.Exception("Fire Girl Shader type not set???");
        }
    }
}
