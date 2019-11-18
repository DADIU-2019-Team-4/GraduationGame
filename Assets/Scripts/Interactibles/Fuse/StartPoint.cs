using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private Fuse fuse;
    private TeleportFuse teleportFuse;

    public enum PointType
    {
        Start,
        End
    }

    public PointType pointType;

    private void Start()
    {
        fuse = GetComponentInParent<Fuse>();
        teleportFuse = GetComponentInParent<TeleportFuse>();
    }

    public void StartFollowingFuse()
    {
        if (fuse != null)
            fuse.Follow(pointType);
        else
            teleportFuse.Follow(pointType);
    }
}
