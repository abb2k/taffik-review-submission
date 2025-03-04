using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordController : MonoBehaviour
{
    public Discord.Discord discord;

    public long ClientID;
    public DiscordPresence Presence;

    public static DiscordController Inctance;

    long time;


    private void Awake()
    {
        if (Inctance == null)
        {
            Inctance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static DiscordController get() { return Inctance; }

    // Use this for initialization
    void Start()
    {
        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        discord = new Discord.Discord(ClientID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
    }

    // Update is called once per frame
    void Update()
    {
        if (discord != null)
            discord.RunCallbacks();
    }

    public void SetPrecense(DiscordPresence presence, bool resetElapsedTime)
    {
        if (discord == null) return;

        Presence = presence;

        if (resetElapsedTime)
        {
            time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = Presence.State,
            Details = Presence.Details,
            Timestamps =
            {
                Start = time,
            },
            Assets =
            {
                LargeImage = Presence.LargeImage,
                LargeText = Presence.LargeText,

                SmallImage = Presence.SmallImage,
                SmallText = Presence.SmallText
            }

        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                //Debug.LogError("Everything is fine!");
            }
        });
    }

    public void ResetElipsedTime()
    {
        if (discord == null) return;

        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = Presence.State,
            Details = Presence.Details,
            Timestamps =
            {
                Start = time,
            },
            Assets =
            {
                LargeImage = Presence.LargeImage,
                LargeText = Presence.LargeText,

                SmallImage = Presence.SmallImage,
                SmallText = Presence.SmallText
            }

        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res == Discord.Result.Ok)
            {
                //Debug.LogError("Everything is fine!");
            }
        });
    }
}
