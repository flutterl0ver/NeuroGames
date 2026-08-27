using System;
using System.Linq;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public static ServerManager singleton;
    public GameObject ServerGroup;

    private void Start()
    {
        ServerGroup.SetActive(false);
        singleton = this;
    }

    public void SpawnServers()
    {
        ServerGroup.SetActive(true);
        var servers = FindObjectsOfType<Server>().ToList();
        System.Random rnd = new();
        var picked = servers.OrderBy(x => rnd.Next()).Take(3);
        foreach (Server s in picked)
        {
            s.Activate();
        }
    }
}
