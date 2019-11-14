using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnObject : MonoBehaviour
{
    [SerializeField]
    private bool _isburnable;
    private bool _isBurning = false;
    private bool _isDissasembling = true;
    //[SerializeField]
    //private List <BurnObject> burnNeighbors;
    private float _burnedAmount;
    Material shader;
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
    [Range(0, 1)]
    public float burnSpeed = 0.1f;
    public bool destroyAtTheEndOfFire = true;
    public void OnEnable()
    {
        shader = gameObject.GetComponent<Renderer>().material;
        movementController = FindObjectOfType<MovementController>();

    }
    /* public override void GameLoopUpdate()
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
     }*/

    public void Update()
    {
        if (_isBurning)
        {
            if (firetype == FireType.Short)
                ShortFire();
            else if (firetype == FireType.Long)
                ShortFire();
            else if (firetype == FireType.Infinity && fireFloat < -0.42f)
                InfinityFire();
        }
    }

    public void SetObjectOnFire(Vector3 collisionPoint)
    {
        _isBurning = true;
        gameObject.GetComponent<Collider>().enabled = false;
        shader = gameObject.GetComponent<Renderer>().material;
        shader.SetVector("_StartPoint", gameObject.transform.position);
        fireObject = gameObject.transform.Find("Fire");
        fireObject.gameObject.SetActive(true);
        fireObject.GetComponent<Renderer>().material.SetVector("_StartPoint", gameObject.transform.position);
        fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);

    }

    /*private void LongFire()
    {

        
        if (!_isDissasembling)
        {
            if (fireFloat >= -0.42f)
            {
                _isDissasembling = true;
                _burnedAmount = 1.15f;
                shader.SetFloat("_T", _burnedAmount);
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
                shader.SetFloat("_T", _burnedAmount);
                fireFloat -= 0.02f;
                fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
            }
            else if (destroyAtTheEndOfFire)
            {
                Destroy(gameObject);
                //RemoveFromGameLoop();
            }
        }
    }*/
    private void ShortFire()
    {
        shader.SetVector("_StartPoint", gameObject.transform.position); 

        if (!_isDissasembling)
        {
            if (fireFloat >= -0.42f)
            {
                _isDissasembling = true;
                _burnedAmount = 1.15f;
                shader.SetFloat("_T", _burnedAmount);
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
                _burnedAmount += burnSpeed;
                fireFloat -= 0.05f;
                shader.SetFloat("_T", _burnedAmount);
                fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
            }
            else if (destroyAtTheEndOfFire)
            {
                Destroy(gameObject);

                //RemoveFromGameLoop();
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
