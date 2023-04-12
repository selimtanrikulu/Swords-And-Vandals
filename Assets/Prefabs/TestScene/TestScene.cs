using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class TestScene : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup heldItemsGrid;
    private IItemManager _itemManager;
    private HeldItemPack _heldItemPack;

    [SerializeField] private GameObject gridItemPrefab;

    [SerializeField] private Button rightHandItemButton;
    [SerializeField] private Button leftHandItemButton;

    private HeldItem _selectedItem;

    [Inject]
    void Inject(IItemManager itemManager,HeldItemPack heldItemPack)
    {
        _heldItemPack = heldItemPack;
        _itemManager = itemManager;
    }
    
    void Start()
    {
        rightHandItemButton.onClick.AddListener(RightHandItemButtonOnClick);
        leftHandItemButton.onClick.AddListener(LeftHandItemButtonOnClick);
        
        SelfUpdate();
    }

    private void RightHandItemButtonOnClick()
    {
        _itemManager.SetWearedHeldItem(_selectedItem,HeldItemHold.Right);
        SelfUpdate();
    }

    private void LeftHandItemButtonOnClick()
    {
        _itemManager.SetWearedHeldItem(_selectedItem,HeldItemHold.Left);
        SelfUpdate();
    }


    public void SelfUpdate()
    {
        DestroyHeldItems();
        CreateHeldItems();
        DestroyWearedHeldItems();
        CreateWearedHeldItems();
    }

    private void DestroyWearedHeldItems()
    {

        Destroy(leftHandItemButton.GetComponentInChildren<GridItem>()?.gameObject);
        Destroy(rightHandItemButton.GetComponentInChildren<GridItem>()?.gameObject);
    }

    private void CreateWearedHeldItems()
    {
        HeldItem left = _itemManager.GetWearedHeldItem(HeldItemHold.Left);
        HeldItem right = _itemManager.GetWearedHeldItem(HeldItemHold.Right);
        
        if (right != null)
        {
            GameObject gridItem = Instantiate(gridItemPrefab, new Vector3(),Quaternion.identity);
            gridItem.transform.SetParent(rightHandItemButton.transform,false);
            GameObject heldItemGameObject = Instantiate(right.heldItemPrefab, new Vector3(), Quaternion.identity);
            heldItemGameObject.transform.SetParent(gridItem.transform,false);
            heldItemGameObject.transform.localScale = new Vector3(200, 200, 200);
            heldItemGameObject.transform.eulerAngles = new Vector3(0, 0, 30);
            heldItemGameObject.transform.localPosition = new Vector3(0, -30, 0);
            gridItem.GetComponent<GridItem>().self = right;
        }

        if (left != null)
        {
            GameObject gridItem = Instantiate(gridItemPrefab, new Vector3(),Quaternion.identity);
            gridItem.transform.SetParent(leftHandItemButton.transform,false);
            GameObject heldItemGameObject = Instantiate(left.heldItemPrefab, new Vector3(), Quaternion.identity);
            heldItemGameObject.transform.SetParent(gridItem.transform,false);
            heldItemGameObject.transform.localScale = new Vector3(200, 200, 200);
            heldItemGameObject.transform.eulerAngles = new Vector3(0, 0, 30);
            heldItemGameObject.transform.localPosition = new Vector3(0, -30, 0);
            gridItem.GetComponent<GridItem>().self = left;
        }
    }

    public void ToggleHeldItem(HeldItem heldItem)
    {
        
        //item already weared
        if (_itemManager.GetWearedHeldItem(HeldItemHold.Left) == heldItem)
        {
            _itemManager.ResetWearedHeldItem(HeldItemHold.Left);
        }
        else if (_itemManager.GetWearedHeldItem(HeldItemHold.Right) == heldItem)
        {
            _itemManager.ResetWearedHeldItem(HeldItemHold.Right);
        }
        //---------------
        else
        {
            _selectedItem = heldItem;
        }
        SelfUpdate();
    }

    private void CreateHeldItems()
    {
        foreach (HeldItem heldItem in _heldItemPack.heldItems)
        {
            
            if(_itemManager.GetWearedHeldItem(HeldItemHold.Right) == heldItem || _itemManager.GetWearedHeldItem(HeldItemHold.Left) == heldItem)continue;

            GameObject gridItem = Instantiate(gridItemPrefab, new Vector3(),Quaternion.identity);
            gridItem.transform.SetParent(heldItemsGrid.transform,false);

            GameObject heldItemGameObject = Instantiate(heldItem.heldItemPrefab, new Vector3(), Quaternion.identity);
            heldItemGameObject.transform.SetParent(gridItem.transform,false);

            heldItemGameObject.transform.localScale = new Vector3(200, 200, 200);
            heldItemGameObject.transform.eulerAngles = new Vector3(0, 0, 30);
            heldItemGameObject.transform.localPosition = new Vector3(0, -30, 0);

            gridItem.GetComponent<GridItem>().self = heldItem;
        }
    }

    private void DestroyHeldItems()
    {
        GridItem[] gridItems = heldItemsGrid.GetComponentsInChildren<GridItem>();
        foreach(GridItem gridItem in gridItems)
        {
            Destroy(gridItem.gameObject);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Arena");
    }
    
    
    private static Sprite ConvertToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }
}
