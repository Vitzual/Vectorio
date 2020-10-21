﻿using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Building : MonoBehaviour
{

    // Placement sprites
    public Sprite Turret;
    public Sprite Sniper;
    public Sprite Enemy;
    public Sprite Bolt;
    public Sprite Shotgun;
    public Sprite SMG;
    public Sprite Wall;
    private SpriteRenderer Selected;
    private float Adjustment = 1f;
    private int AdjustLimiter = 0;
    private bool AdjustSwitch = false;
    private bool QuickPlace = true;

    // Object placements
    [SerializeField]
    private GameObject GridObj;
    [SerializeField]
    private GameObject SniperObj;
    [SerializeField]
    private GameObject TurretObj;
    [SerializeField]
    private GameObject EnemyObj;
    [SerializeField]
    private GameObject BoltObj;
    [SerializeField]
    private GameObject ShotgunObj;
    [SerializeField]
    private GameObject WallObj;
    [SerializeField]
    private GameObject SMGObj;
    private GameObject SelectedObj;
    private GameObject LastObj;

    // UI Elements
    public Canvas Overlay;
    private bool MenuOpen;

    // Internal placement variables
    [SerializeField]
    private LayerMask TileLayer;
    private Vector2 MousePos;

    private void Start()
    {
        Selected = GetComponent<SpriteRenderer>();
        MenuOpen = false;
    }

    private void Update()
    {
        // Get mouse position and round to middle grid coordinate
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(5*Mathf.Round(MousePos.x/5), 5*Mathf.Round(MousePos.y/5));

        // Make color flash
        Color tmp = this.GetComponent<SpriteRenderer>().color;
        tmp.a = Adjustment;
        this.GetComponent<SpriteRenderer>().color = tmp;
        AdjustAlphaValue();

        if (Input.mousePosition.y > 170f)
        {
            if (QuickPlace == true)
            {
                // If user left clicks, place object
                if (Input.GetButton("Fire1") && SelectedObj != null)
                {

                    RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

                    // Raycast tile to see if there is already a tile placed
                    if (rayHit.collider == null)
                    {
                        LastObj = Instantiate(SelectedObj, transform.position, Quaternion.identity);
                        if (SelectedObj == WallObj)
                        {
                            CalculateWallPlacement();
                        }
                    }
                }

                // If user right clicks, place object
                else if (Input.GetButton("Fire2") && SelectedObj != null)
                {

                    RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

                    // Raycast tile to see if there is already a tile placed
                    if (rayHit.collider != null && rayHit.collider.name != "Hub")
                    {
                        Destroy(rayHit.collider.gameObject);
                    }
                }
            }
            else if (QuickPlace == false)
            {
                // If user left clicks, place object
                if (Input.GetButtonDown("Fire1"))
                {

                    RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

                    // Raycast tile to see if there is already a tile placed
                    if (rayHit.collider == null)
                    {
                        LastObj = Instantiate(SelectedObj, transform.position, Quaternion.identity);
                        if (SelectedObj == WallObj)
                        {
                            CalculateWallPlacement();
                        }
                    }
                }

                // If user right clicks, place object
                else if (Input.GetButtonDown("Fire2"))
                {

                    RaycastHit2D rayHit = Physics2D.Raycast(MousePos, Vector2.zero, Mathf.Infinity, TileLayer);

                    // Raycast tile to see if there is already a tile placed
                    if (rayHit.collider != null && rayHit.collider.name != "Hub")
                    {
                        Destroy(rayHit.collider.gameObject);
                    }
                }
            }
        }

        // Change selected object
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetTurret();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetTriangle();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetShotgun();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSniper();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetSMG();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetBolt();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetWall();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            QuickPlace = !QuickPlace;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && SelectedObj != null)
        {
            DisableActiveInfo();
            Selected.sprite = null;
            SelectedObj = null;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && MenuOpen == false)
        {
            MenuOpen = true;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().alpha = 1;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().blocksRaycasts = true;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().interactable = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuOpen = false;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().alpha = 0;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().blocksRaycasts = false;
            Overlay.transform.Find("Return").GetComponent<CanvasGroup>().interactable = false;
        }
    }

    void CalculateWallPlacement()
    {
        RaycastHit2D a = Physics2D.Raycast(new Vector2(MousePos.x+5f,MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D b = Physics2D.Raycast(new Vector2(MousePos.x-5f,MousePos.y), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D c = Physics2D.Raycast(new Vector2(MousePos.x,MousePos.y+5f), Vector2.zero, Mathf.Infinity, TileLayer);
        RaycastHit2D d = Physics2D.Raycast(new Vector2(MousePos.x,MousePos.y-5f), Vector2.zero, Mathf.Infinity, TileLayer);
        if (a.collider != null && a.collider.name == "Wall(Clone)")
        {
            a.collider.GetComponent<WallAI>().UpdateSprite(1);
            LastObj.GetComponent<WallAI>().UpdateSprite(3);
        }
        if (b.collider != null && b.collider.name == "Wall(Clone)")
        {
            b.collider.GetComponent<WallAI>().UpdateSprite(3);
            LastObj.GetComponent<WallAI>().UpdateSprite(1);
        }
        if (c.collider != null && c.collider.name == "Wall(Clone)")
        {
            c.collider.GetComponent<WallAI>().UpdateSprite(2);
            LastObj.GetComponent<WallAI>().UpdateSprite(4);
        }
        if (d.collider != null && d.collider.name == "Wall(Clone)")
        {
            d.collider.GetComponent<WallAI>().UpdateSprite(4);
            LastObj.GetComponent<WallAI>().UpdateSprite(2);
        }
    }

    public void AdjustAlphaValue()
    {
        if (AdjustLimiter == 10)
        {
            if (AdjustSwitch == false)
            {
                Adjustment -= 0.1f;
            }
            else if (AdjustSwitch == true)
            {
                Adjustment += 0.1f;
            }
            if (AdjustSwitch == false && Adjustment <= 0f)
            {
                AdjustSwitch = true;
            }
            else if (AdjustSwitch == true && Adjustment >= 1f)
            {
                AdjustSwitch = false;
            }
            AdjustLimiter = 0;
        }
        else
        {
            AdjustLimiter += 1;
        }
    }

    public void SetTurret()
    {
        DisableActiveInfo();
        Overlay.transform.Find("Turret Info").GetComponent<CanvasGroup>().alpha = 1;
        Adjustment = 1f;
        Selected.sprite = Turret;
        SelectedObj = TurretObj;
    }

    public void SetShotgun()
    {
        DisableActiveInfo();
        Overlay.transform.Find("Shotgun Info").GetComponent<CanvasGroup>().alpha = 1;
        Adjustment = 1f;
        Selected.sprite = Shotgun;
        SelectedObj = ShotgunObj;
    }

    public void SetSniper()
    {
        DisableActiveInfo();
        Overlay.transform.Find("Sniper Info").GetComponent<CanvasGroup>().alpha = 1;
        Adjustment = 1f;
        Selected.sprite = Sniper;
        SelectedObj = SniperObj;
    }

    public void SetSMG()
    {
        DisableActiveInfo();
        Overlay.transform.Find("SMG Info").GetComponent<CanvasGroup>().alpha = 1;
        Adjustment = 1f;
        Selected.sprite = SMG;
        SelectedObj = SMGObj;
    }

    public void SetBolt()
    {
        DisableActiveInfo();
        Overlay.transform.Find("Pulser Info").GetComponent<CanvasGroup>().alpha = 1;
        Adjustment = 1f;
        Selected.sprite = Bolt;
        SelectedObj = BoltObj;
    }

    public void SetWall()
    {
        DisableActiveInfo();
        Overlay.transform.Find("Wall Info").GetComponent<CanvasGroup>().alpha = 1;
        Adjustment = 1f;
        Selected.sprite = Wall;
        SelectedObj = WallObj;
    }

    public void SetTriangle()
    {
        DisableActiveInfo();
        Adjustment = 1f;
        Selected.sprite = Enemy;
        SelectedObj = EnemyObj;
    }

    public void DisableActiveInfo()
    {
        Overlay.transform.Find("Turret Info").GetComponent<CanvasGroup>().alpha = 0;
        Overlay.transform.Find("Shotgun Info").GetComponent<CanvasGroup>().alpha = 0;
        Overlay.transform.Find("Sniper Info").GetComponent<CanvasGroup>().alpha = 0;
        Overlay.transform.Find("SMG Info").GetComponent<CanvasGroup>().alpha = 0;
        Overlay.transform.Find("Pulser Info").GetComponent<CanvasGroup>().alpha = 0;
        Overlay.transform.Find("Wall Info").GetComponent<CanvasGroup>().alpha = 0;
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

}