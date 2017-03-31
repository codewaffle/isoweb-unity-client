using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;

namespace isoweb
{
    class RecipePart
    {
        public int Count;
        public string Hash;
    }

    class RecipeData
    {
        public string Hash;
        public string Name;
        public string Description;
        public bool Loaded;
        public List<RecipePart> Consumes;
        public List<RecipePart> Tools;
    }

    public class CraftInterface : MonoBehaviour
    {
        public RectTransform RecipeButtonPrefab;
        public RectTransform ExecuteButton;

        Dictionary<string, RecipeData> _recipeData = new Dictionary<string, RecipeData>();
        RecipeData _currentRecipeData = null;

        private Transform _recipeListContent;
        private Transform _infoRoot;
        private Text _infoName;
        private Text _infoTools;
        private Text _infoDesc;
        private Text _infoConsumes;

        void Start()
        {
            Debug.Log("Registering CraftInterface");
            Global.CraftInterface = this;
            Global.Client.RegisterPacketHandler(PacketType.CRAFT_LIST, UpdateCraftList);
            Global.Client.RegisterPacketHandler(PacketType.CRAFT_VIEW, UpdateCraftView);

            _recipeListContent = transform.FindFirstRecursive("RecipeListContent");
            _infoRoot = transform.FindFirstRecursive("RecipeInfo");
            _infoName = _infoRoot.FindFirstRecursive("ItemName").GetComponent<Text>();
            _infoDesc = _infoRoot.FindFirstRecursive("ItemDescription").GetComponent<Text>();
            _infoConsumes = _infoRoot.FindFirstRecursive("ItemIngredients").GetComponent<Text>();
            _infoTools = _infoRoot.FindFirstRecursive("ItemTools").GetComponent<Text>();
            ClearRecipe();
        }

        private void UpdateCraftView(PacketReader pr)
        {
            var obj = pr.ReadJsonObject().AsObject;
            Debug.Log(obj);
            var hexHash = obj["hash"].Value;
            var data = _recipeData[hexHash];
            data.Name = obj["name"];
            data.Description = obj["description"];
            data.Loaded = true;
        }

        private void UpdateCraftList(PacketReader pr)
        {
            bool create;

            var obj = pr.ReadJsonObject();
            foreach (JSONArray rec in obj.AsArray)
            {
                var data = new RecipeData
                {
                    Hash = rec[0],
                    Name = rec[1]
                };

                create = !_recipeData.ContainsKey(data.Hash);
                _recipeData[data.Hash] = data;

                if (create)
                {
                    var butt = Instantiate(RecipeButtonPrefab);
                    butt.SetParent(_recipeListContent);
                    Debug.Log(_recipeListContent);
                    butt.GetComponentInChildren<Text>().text = data.Name;

                    butt.GetComponentInChildren<Button>().onClick.AddListener(() => SetRecipe(data.Hash));
                }

                Debug.Log("WOOT " + data.Hash + " / " + data.Name);
            }

            RefreshCraftList();
        }

        private void SetRecipe(string hash)
        {
            var data = _recipeData[hash];

            if (!data.Loaded)
            {
                StartCoroutine(LoadRecipe(data));
            }
            else
            {
                DisplayRecipe(data);
            }
        }

        private void DisplayRecipe(RecipeData data)
        {
            _infoName.text = data.Name;
            _infoDesc.text = data.Description;
            _infoConsumes.text = "";
            _infoTools.text = "";
            _currentRecipeData = data;
            ExecuteButton.gameObject.SetActive(true);
        }

        private void ClearRecipe()
        {
            _infoName.text = _infoDesc.text = _infoConsumes.text = _infoTools.text = "";
            ExecuteButton.gameObject.SetActive(false);
            _currentRecipeData = null;
        }

        IEnumerator LoadRecipe(RecipeData data)
        {
            Global.Client.RequestCraftRecipe(data.Hash);

            while (!data.Loaded)
                yield return null;

            DisplayRecipe(data);
        }


        private void RefreshCraftList()
        {

        }

        void OnEnable()
        {
            Debug.Log("OnEnable");
        }

        void Update()
        {
            var rt = (RectTransform)transform;
            rt.anchoredPosition = Vector2.zero;
        }

        void OnExpandTab()
        {
            Global.Client.RequestCraftList();
        }

        public void OnExecuteClicked()
        {
            Global.Client.RequestCraftExecute(_currentRecipeData.Hash);
        }
    }
}