using DynamicTrees.Utilities;
using static Il2Cpp.AkMIDIEvent;
using static Il2CppSystem.DateTimeParse;

namespace DynamicTrees.DynamicTreesComponent;

[RegisterTypeInIl2Cpp]
internal class DynamicTreeData : MonoBehaviour
{

    //WeatherStage previousWeather;
    WeatherStage currentWeather;
    WindStrength currentWind;

    public float clearAccumulation = 0; //0cm
    public float lowestAccumulation = 5; //5cm
    public float lowAccumulation = 15; //10cm
    public float lowMediumAccumulation = 40; //15cm
    public float mediumAccumulation = 65; //25cm
    public float mediumHighAccumulation = 80; //35cm
    public float highAccumulation = 85; //45cm
    public float highestAccumulation = 95; //50cm
    public float fullAccumulation = 100; //55-60cm

    public string scene = "";
    public bool hasInstancedTrees;
    public enum state { Clear, Lowest, Low, LowMed, Med, MedHigh, High, Highest, Full };

    public state currentState;

    float currentAccumulation = 0;
    float accumulationAmountPerHour = 0;

    public void Awake()
    {
        currentWeather = GameManager.GetWeatherComponent().GetWeatherStage();
        currentWind = GameManager.GetWindComponent().GetStrength();
        scene = GameManager.m_ActiveScene;
        LoadAndSaveData();
    }

    public void Start()
    {
    }

    public void Update()
    {

        currentWeather = GameManager.GetWeatherComponent().GetWeatherStage();
        currentWind = GameManager.GetWindComponent().GetStrength();

        float tODHours = GameManager.GetTimeOfDayComponent().GetTODHours(Time.deltaTime);
        Accumulate(tODHours);

        state prevState = currentState;
        currentState = SetState();

        if (HitThreshold(prevState))
        {
            TextureHelper.ReplaceTreeTextures(scene, hasInstancedTrees);
        }

    }

    public void Accumulate(float numHoursDelta)
    {
        accumulationAmountPerHour = CalculateAccumulationAmountPerHour();
        currentAccumulation += accumulationAmountPerHour * numHoursDelta;

        currentAccumulation = Mathf.Clamp(currentAccumulation, 0f, 100f);
    }
    public float CalculateAccumulationAmountPerHour()
    {

        float accumulationAmount = 0;

        if (IsNotSnowing())
        {
            accumulationAmount = 0;

            if (currentAccumulation >= 80f) accumulationAmount = -10f;

            if (currentWind == WindStrength.SlightlyWindy) accumulationAmount -= 2f;
            else if (currentWind == WindStrength.Windy) accumulationAmount -= 5f;
            else if (currentWind == WindStrength.VeryWindy) accumulationAmount -= 10f;
            else if (currentWind == WindStrength.Blizzard) accumulationAmount -= 20f;
        }
        else if (currentWeather == WeatherStage.LightSnow)
        {
            accumulationAmount = 3f;
            if (currentWind == WindStrength.SlightlyWindy) accumulationAmount += 1f;
            else if (currentWind == WindStrength.Windy) accumulationAmount += 3f;
            else if (currentWind == WindStrength.VeryWindy) accumulationAmount += 5f;
            else if (currentWind == WindStrength.Blizzard) accumulationAmount += 7f;
        }
        else if (currentWeather == WeatherStage.HeavySnow)
        {
            accumulationAmount = 7f;
            if (currentWind == WindStrength.SlightlyWindy) accumulationAmount += 3f;
            else if (currentWind == WindStrength.Windy) accumulationAmount += 6f;
            else if (currentWind == WindStrength.VeryWindy) accumulationAmount += 8f;
            else if (currentWind == WindStrength.Blizzard) accumulationAmount += 10f;
        }
        else if (currentWeather == WeatherStage.Blizzard)
        {
            accumulationAmount = 30f;
        }

        return accumulationAmount;
    }
    public float GetCurrentAccumulation()
    {
        return currentAccumulation;
    }
    public float GetStartingAccumulation()
    {
        if (IsNotSnowing()) return clearAccumulation;
        else if (currentWeather == WeatherStage.LightSnow) return lowestAccumulation;
        else if (currentWeather == WeatherStage.HeavySnow) return mediumAccumulation;
        else if (currentWeather == WeatherStage.Blizzard) return highAccumulation;
        else return clearAccumulation;
    }
    public bool IsNotSnowing()
    {
        if (currentWeather != WeatherStage.LightSnow && currentWeather != WeatherStage.HeavySnow && currentWeather != WeatherStage.Blizzard) return true;
        else return false;
    }
    public bool HitThreshold(state prevState)
    {

        if (accumulationAmountPerHour == 0 || GameManager.GetWeatherComponent().IsIndoorScene()) return false;

        if (prevState == currentState) return false;
        else return true;
    }
    private state SetState()
    {
        if (currentAccumulation >= clearAccumulation && currentAccumulation < lowestAccumulation) return state.Clear;
        else if (currentAccumulation >= lowestAccumulation && currentAccumulation < lowAccumulation) return state.Lowest;
        else if (currentAccumulation >= lowAccumulation && currentAccumulation < lowMediumAccumulation) return state.Low;
        else if (currentAccumulation >= lowMediumAccumulation && currentAccumulation < mediumAccumulation) return state.LowMed;
        else if (currentAccumulation >= mediumAccumulation && currentAccumulation < mediumHighAccumulation) return state.Med;
        else if (currentAccumulation >= mediumHighAccumulation && currentAccumulation < highAccumulation) return state.MedHigh;
        else if (currentAccumulation >= highAccumulation && currentAccumulation < highestAccumulation) return state.High;
        else if (currentAccumulation >= highestAccumulation && currentAccumulation < fullAccumulation) return state.Highest;
        else return state.Full;
    }
    public void LoadAndSaveData()
    {
        SaveDataManager sdm = Main.sdm;
        DynamicTreeSaveDataProxy? dataToSaveOrLoad;

        dataToSaveOrLoad = sdm.Load();

        if(dataToSaveOrLoad == null)
        {
            currentAccumulation = GetStartingAccumulation();

            dataToSaveOrLoad = new DynamicTreeSaveDataProxy();
            dataToSaveOrLoad.savedAccumulation = currentAccumulation;
        }
        else
        {
            currentAccumulation = dataToSaveOrLoad.savedAccumulation;
        }
        
        sdm.Save(dataToSaveOrLoad);
    }
    public void SaveData()
    {
        SaveDataManager sdm = Main.sdm;
        DynamicTreeSaveDataProxy? dataToSave = new DynamicTreeSaveDataProxy();

        dataToSave.savedAccumulation = currentAccumulation;
        sdm.Save(dataToSave);
    }

}
