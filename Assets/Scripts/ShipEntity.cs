using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEntity : MonoBehaviour
{
    #region Properties
    public double Shields
    {
        get => _Shields;
        set
        {
            _Shields = value;
        }
    }
    private double _Shields;

    public double Supplies
    {
        get => _Supplies;
        set
        {
            _Supplies = value;
        }
    }
    private double _Supplies;

    public double Fuel
    {
        get => _Fuel;
        set
        {
            _Fuel = value;
        }
    }
    private double _Fuel;
    #endregion

    public static ShipEntity Instance;

    // Start is called before the first frame update
    void Start()
    {
        Shields = 100.00;
        Supplies = 100.00;
        Fuel = 100.00;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Ensures that the ship instance does not get destroyed when loading a new scene
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
