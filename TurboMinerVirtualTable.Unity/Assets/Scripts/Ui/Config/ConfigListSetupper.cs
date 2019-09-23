using UnityEngine;
using UnityEngine.UI;

public class ConfigListSetupper : MonoBehaviour
{
    public ConfigMenu ConfigMenu;
    public ConfigLoader ConfigLoader;

    public void Setup(string subPath)
    {
        var configNameButton = Resources.Load<Button>("Prefabs/ConfigNameButton");
        var configNames = ConfigLoader.GetConfigNames(subPath);

        if(configNames == null)
        {
            return;
        }

        var buttons = GetComponentsInChildren<Button>();
        foreach(var button in buttons)
        {
            Destroy(button.gameObject);
        }

        for(var i = 0; i < configNames.Length; ++i)
        {
            var configNameButtonInstance = Instantiate(configNameButton, configNameButton.transform.position, Quaternion.identity, ConfigMenu.ConfigList.content.transform);
            var text = configNameButtonInstance.GetComponentInChildren<Text>();
            text.text = configNames[i];

            configNameButtonInstance.transform.localPosition = new Vector3(configNameButtonInstance.transform.localPosition.x+50, -i*30-20, 0);
            configNameButtonInstance.GetComponent<Button>().onClick.AddListener(() => 
            {
                ConfigMenu.LoadConfig(subPath, text.text);
            });
        }
    }
}
