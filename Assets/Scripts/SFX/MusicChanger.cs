using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    public MusicManager musicManager;
    public GameObject player1; // Reference to the player object
    public Character player;

    public SceneManager sceneManager;

    private bool prevIsDay;
    private bool prevIsIndoor;

    private bool prevGoingUnderground;

    void Start()
    {
        if (player1 != null)
        {
            player = player1.GetComponent<Character>();
        }
        if (musicManager != null)
        {
            prevIsDay = true;
            prevIsIndoor = player.isIndoors;
            musicManager.ChangeMusic(prevIsDay, prevIsIndoor, prevGoingUnderground);
        }
    }

    public void OnDayChange(bool isDay)
    {
        if (musicManager != null && player != null)
        {
            bool currentIsDay = isDay;

            if (currentIsDay != prevIsDay)
            {
                prevIsDay = currentIsDay;
                musicManager.ChangeMusic(currentIsDay, prevIsIndoor, prevGoingUnderground, 3f);
            }
        }
    }

    public void OnPlayerLocationChange(bool isIndoors, bool isGoingUnderground)
    {
        if (musicManager != null && player != null)
        {
            if ((isIndoors != prevIsIndoor) || (isGoingUnderground != prevGoingUnderground))
            {
                prevIsIndoor = isIndoors;
                prevGoingUnderground = isGoingUnderground;
                musicManager.ChangeMusic(prevIsDay, isIndoors, isGoingUnderground, .5f);
            }
        }
    }
}
