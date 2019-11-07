using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnObject : MonoBehaviour
{
    [SerializeField]
    private bool _isburnable;
    private bool _isBurning = false;
    //[SerializeField]
    //private List <BurnObject> burnNeighbors;
    private float _burnedAmount;
    Material shader;

    public enum FireType
    {
        Long,
        Short,
        Infinity
    }
    public FireType firetype;
    public void OnEnable()
    {
        shader = gameObject.GetComponent<Renderer>().material;
    }
    public  void Update()
    {
        if (_isBurning)
        {
            if (firetype == FireType.Short)
                ShortFire();
            else if (firetype == FireType.Long)
                LongFire();
            else
                return;

               

        }
    }

    public void SetObjectOnFire()
    {
        _isBurning = true;
        gameObject.GetComponent<Collider>().enabled = false;
        var shader = gameObject.GetComponent<Renderer>().material;
        _burnedAmount = 0f;
        shader.SetFloat("_DissolveAmount", _burnedAmount);
        var fireObject = gameObject.transform.GetChild(0);
        fireObject.gameObject.SetActive(true);
    }

    private void LongFire()
    {
        if (_burnedAmount < 1f)
        {
            _burnedAmount += 0.0001f;
            shader.SetFloat("_DissolveAmount", _burnedAmount);
        }
        if (_burnedAmount >= 0.95f)
            Destroy(gameObject);
    }
    private void ShortFire()
    {
        if (_burnedAmount < 1f)
        {
            _burnedAmount += 0.01f;
            shader.SetFloat("_DissolveAmount", _burnedAmount);
        }
        if (_burnedAmount >= 0.95f)
            Destroy(gameObject);
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
}
