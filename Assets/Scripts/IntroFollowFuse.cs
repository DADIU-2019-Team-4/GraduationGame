using UnityEngine;

public class IntroFollowFuse : MonoBehaviour
{
    public Fuse Fuse;
    private bool _firstTime = true;

    // Update is called once per frame
    void Update()
    {
        if (_firstTime)
        {
            _firstTime = false;
            Fuse.Follow(StartPoint.PointType.Start);
        }
    }
}
