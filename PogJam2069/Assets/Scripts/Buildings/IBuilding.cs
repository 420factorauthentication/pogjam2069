using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    string BuildingName { get; set; }
    int Cost { get; set; }
    bool IsBuilt { get; set; }


    void CheckCanBuild(int currWood);
    void BuildBuilding();
}
