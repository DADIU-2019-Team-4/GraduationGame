using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private Fuse fuse;

    public enum PointType
    {
        Start,
        End
    }

    public PointType pointType;

    private void Start()
    {
        fuse = GetComponentInParent<Fuse>();
    }

    public void StartFollowingFuse()
    {
        fuse.Follow(pointType);
    }
}
