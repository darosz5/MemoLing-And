using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System.Linq;

public class IAPManager : MonoBehaviour, IStoreListener {

    public static IAPManager Instance;

    static IStoreController m_storeController;
    static IExtensionProvider m_storeExtensionProvider;

    public static string productId1EnglishJobs =   "english_jobs";
    public static string productId1EnglishTravel = "english_travel";
    public static string productId1EnglishSport =  "english_sport";

    public static string productId1GermanJobs =   "german_jobs";
    public static string productId1GermanTravel = "german_travel";
    public static string productId1GermanSport =  "german_sport";

    public static string productId1FrenchJobs =   "french_jobs";
    public static string productId1FrenchTravel = "frencht_ravel";
    public static string productId1FrenchSport =  "french_sport";

    public static string productId1SpanishJobs =   "spanish_jobs";
    public static string productId1SpanishTravel = "spanish_travel";
    public static string productId1SpanishSport =  "spanish_sport";

    public static string productId1PolishJobs =   "polish_jobs";
    public static string productId1PolishTravel = "polish_travel";
    public static string productId1PolishSport =  "polish_sport";

    public string language;

    private IAPModul m_IAPModul;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!IsInitialized())
        {
            InitializePurchasing();
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_storeController = controller;
        m_storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogWarning("IAPMANAGER OnInitializeFailed Error: " + error);        
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.Log("IAPMANAGER OnPurchaseFailed for product " + i.definition.storeSpecificId + "  PurchaseFailureReason: " + p);       
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        
        string productId = e.purchasedProduct.definition.id;

        EnableProduct(productId);

        return PurchaseProcessingResult.Complete;

    }

    // ----------------------------------
    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        StandardPurchasingModule module = StandardPurchasingModule.Instance();

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

        builder.AddProduct(productId1EnglishJobs, ProductType.NonConsumable);
        builder.AddProduct(productId1EnglishSport, ProductType.NonConsumable);
        builder.AddProduct(productId1EnglishTravel, ProductType.NonConsumable);

        builder.AddProduct(productId1FrenchJobs, ProductType.NonConsumable);
        builder.AddProduct(productId1FrenchSport, ProductType.NonConsumable);
        builder.AddProduct(productId1FrenchTravel, ProductType.NonConsumable);

        builder.AddProduct(productId1GermanJobs, ProductType.NonConsumable);
        builder.AddProduct(productId1GermanSport, ProductType.NonConsumable);
        builder.AddProduct(productId1GermanTravel, ProductType.NonConsumable);

        builder.AddProduct(productId1PolishJobs, ProductType.NonConsumable);
        builder.AddProduct(productId1PolishSport, ProductType.NonConsumable);
        builder.AddProduct(productId1PolishTravel, ProductType.NonConsumable);

        builder.AddProduct(productId1SpanishJobs, ProductType.NonConsumable);
        builder.AddProduct(productId1SpanishSport, ProductType.NonConsumable);
        builder.AddProduct(productId1SpanishTravel, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    bool IsInitialized()
    {
        return m_storeController != null && m_storeExtensionProvider != null;
    }

    public void BuyProductID(string productId)
    {
        m_IAPModul = FindObjectOfType<IAPModul>();
        if (IsInitialized())
        {
            Product product = m_storeController.products.WithID(productId);

            if (product != null) 
            {
                if (product.hasReceipt && product.definition.type == ProductType.NonConsumable)
                {
                    EnableProduct(productId);
                    Debug.Log("IAPMANAGER " + product.definition.id + " already purchased! Enabling!");
                    m_IAPModul.HandleMessage(2);             
                }
                else if (product.availableToPurchase)
                {
                    m_storeController.InitiatePurchase(product);
                    m_IAPModul.HandleMessage(0);
                    Debug.Log("IAPMANAGER Purchasing product: " + product.definition.id);                  
                }
            }
            else
            {
                m_IAPModul.HandleMessage(1);
                Debug.Log("IAPMANAGER BuyProductID ERROR: Product " + productId + " not found");               
            }
        }
        else
        {
            m_IAPModul.HandleMessage(1);
            Debug.LogWarning("IAPMANAGER BuyProductID ERROR: " + productId + ".  Store not initialized");          
        }
    }

    void EnableProduct(string productId)
    {
        string[] languageAndCategory = productId.Split('_');
        List<string> languageAndCategoryList = languageAndCategory.ToList();

        string categoryName = languageAndCategoryList[languageAndCategoryList.Count - 1];
        languageAndCategoryList.RemoveAt(languageAndCategoryList.Count - 1);

        AddNew(categoryName, languageAndCategoryList);
    } 

    public void AddNew(string category, List<string> languages)
    {       
        CategoryAvaliability categoryAvaliability = IsCategoryAvaliable(category);
      
        if (categoryAvaliability == null)
        {
           // Debug.Log("You dont have this category");
            GameControl.control.avaliableCategories.Add(new CategoryAvaliability(category, languages));
        }
        else
        {
           // Debug.Log("You have this category");
            for (int i = 0; i < languages.Count; i++)
            {
                if (!categoryAvaliability.avaliableLanguages.Contains(languages[i]))
                {
                    categoryAvaliability.avaliableLanguages.Add(languages[i]);
                }
                else
                {
                    Debug.Log("you have this");
                }
            }
           
        }
        GameControl.control.SavePlayerData();

    }

    private CategoryAvaliability IsCategoryAvaliable(string categoryName)
    {
        for (int i = 0; i < GameControl.control.avaliableCategories.Count; i++)
        {
            if (GameControl.control.avaliableCategories[i].categoryName == categoryName)
            {
                return GameControl.control.avaliableCategories[i];
            }
        }
        return null;
    }

}
