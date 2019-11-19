using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnObject : MonoBehaviour
{
    public GameObject FireObject;
    private bool _isBurning = false;
    private bool _isDissasembling = true;
    //[SerializeField]
    //private List <BurnObject> burnNeighbors;
    private float _burnedAmount =0.7f;

    private GameObject _fireObject;
    private float fireFloat = -1f;
    private GameObject _fire;

    public Material dissolveShader;
    private Renderer renderer;
    private Texture texture;


    [Range(0, 1)]
    public float burnSpeed = 0.1f;
    public bool destroyAtTheEndOfFire = true;

    public enum FireType
    {
        Long,
        Short,
        Infinity
    }
    public FireType firetype;

    public void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        texture = renderer.material.mainTexture;
        _fire = Instantiate(FireObject, transform);
        
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
        renderer.material = dissolveShader;
        renderer.material.SetTexture("_maintexture", texture);
        _isBurning = true;

        Collider coll = gameObject.GetComponent<Collider>();
        if (!gameObject.GetComponent<Collider>().isTrigger)
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }
        renderer.material.SetVector("_StartPoint", collisionPoint);
        _fireObject = _fire;
        _fireObject.SetActive(true);
        _fireObject.GetComponent<Renderer>().material.SetVector("_StartPoint", collisionPoint);
        _fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
        //_burnedAmount = renderer.material.GetFloat("_T");

    }

    /*private void LongFire()
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
               _fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
            }
        }
        else
        {
            if (_burnedAmount < 3f)
            {
                _burnedAmount += 0.01f;
                renderer.material.SetFloat("_T", _burnedAmount);
                fireFloat -= 0.02f;
               _fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
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
        renderer.material.SetVector("_StartPoint", gameObject.transform.position); 

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
               _fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
            }
        }
        else if (_isDissasembling)
        {
            if (_burnedAmount < 3f)
            {
                _burnedAmount += burnSpeed;
                fireFloat -= 0.05f;
                renderer.material.SetFloat("_T", _burnedAmount);
               _fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
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
       _fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
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
