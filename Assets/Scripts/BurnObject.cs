using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnObject : MonoBehaviour
{
    [SerializeField]
    private bool _isburnable;
    private bool _isBurning = false;
    private bool _isDissasembling = false;
    //[SerializeField]
    //private List <BurnObject> burnNeighbors;
    private float _burnedAmount;
    private MovementController movementController;

    private Transform fireObject;
    private float fireFloat = -1f;

    public enum FireType
    {
        Long,
        Short,
        Infinity
    }
    public FireType firetype;

    public Material dissolveShader;
    private Renderer renderer;

    public void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        movementController = FindObjectOfType<MovementController>();
    }

    public void Update()
    {
        if (_isBurning)
        {
            if (firetype == FireType.Short)
                ShortFire();
            else if (firetype == FireType.Long)
                LongFire();
            else if (firetype == FireType.Infinity && fireFloat < -0.42f)
                InfinityFire();
        }
    }

    public void SetObjectOnFire(Vector3 collisionPoint)
    {
        renderer.material = dissolveShader;
        _isBurning = true;
        gameObject.GetComponent<Collider>().enabled = false;
        renderer.material.SetVector("_StartPoint", collisionPoint);
        fireObject = gameObject.transform.GetChild(0);
        fireObject.gameObject.SetActive(true);
        fireObject.GetComponent<Renderer>().material.SetVector("_StartPoint", collisionPoint);
        fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);

    }

    private void LongFire()
    {
        if (!_isDissasembling)
        {
            if (fireFloat >= -0.42f)
            {
                _isDissasembling = true;
                _burnedAmount = 1.15f;
                renderer.material.SetFloat("_T", _burnedAmount);
            }
            else
            {
                fireFloat += 0.01f;
                fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
            }
        }
        else
        {
            if (_burnedAmount < 3f)
            {
                _burnedAmount += 0.01f;
                renderer.material.SetFloat("_T", _burnedAmount);
                fireFloat -= 0.02f;
                fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private void ShortFire()
    {
        if (!_isDissasembling)
        {
            if (fireFloat >= -0.42f)
            {
                _isDissasembling = true;
                _burnedAmount = 1.15f;
                renderer.material.SetFloat("_T", _burnedAmount);
            }
            else
            {
                fireFloat += 0.025f;
                fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
            }
        }
        else if (_isDissasembling)
        {
            if (_burnedAmount < 3f)
            {
                _burnedAmount += 0.025f;
                fireFloat -= 0.05f;
                renderer.material.SetFloat("_T", _burnedAmount);
                fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private void InfinityFire()
    {
        if (fireFloat >= -0.42f)
            return;
        fireFloat += 0.05f;
        fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
    }
}
    //if (_burnedAmount >= 0.6f) If need to burn a neighbours
    //{
    // for (int i = 0; i <= burnNeighbors.Count - 1; i++)
    //  if (burnNeighbors[i]._isburnable && !burnNeighbors[i]._isBurning)
    ////  {
    //    burnNeighbors[i].SetObjectOnFire();
    //    burnNeighbors.Remove(burnNeighbors[i]);
    // }
    // }
