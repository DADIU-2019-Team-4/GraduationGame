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
    private float _burnedAmount =0.9f;

    private GameObject _fireObject;
    private float fireFloat = -1f;
    private GameObject _fire;

    public Material dissolveShader;
    private Renderer _renderer;
    private Texture texture;
    private Material _defaultShader;

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
        _renderer = gameObject.GetComponent<Renderer>();
        texture = _renderer.material.mainTexture;
        _fire = Instantiate(FireObject, transform);
        _defaultShader = _renderer.material;
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
        _renderer.material = dissolveShader;
        _renderer.material.SetTexture("_maintexture", texture);
        _isBurning = true;

        Collider coll = gameObject.GetComponent<Collider>();
        if (!coll.isTrigger)
        {
            coll.enabled = false;
        }
        _renderer.material.SetVector("_StartPoint", collisionPoint);
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
        _renderer.material.SetVector("_StartPoint", gameObject.transform.position); 

        if (!_isDissasembling)
        {
            if (fireFloat >= -0.42f)
            {
                _isDissasembling = true;
                _burnedAmount = 1.15f;
                _renderer.material.SetFloat("_T", _burnedAmount);
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
                _renderer.material.SetFloat("_T", _burnedAmount);
               _fireObject.GetComponent<Renderer>().material.SetFloat("_FlameHeight", fireFloat);
            }
            else if (destroyAtTheEndOfFire)
            {
                //Destroy(gameObject);
                gameObject.SetActive(false);
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

    public void ResetBreakable()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        _renderer.material = _defaultShader;
        _isBurning = false;
        _isDissasembling = true;
        _fire.SetActive(false);
        _fireObject?.SetActive(false);
        gameObject.SetActive(true);
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
