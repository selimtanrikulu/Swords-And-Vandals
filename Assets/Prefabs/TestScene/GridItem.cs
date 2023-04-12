using UnityEngine;

public class GridItem : MonoBehaviour
{
    [HideInInspector] public HeldItem self;


    private TestScene _testScene;

    private void Start()
    {
        _testScene = FindObjectOfType<TestScene>();
    }

    public void SelectButtonOnClick()
    {
        _testScene.ToggleHeldItem(self);
    }
    
}
