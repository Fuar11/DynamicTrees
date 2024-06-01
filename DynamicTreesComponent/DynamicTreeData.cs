using DynamicTrees.Utilities;

namespace DynamicTrees.DynamicTreesComponent;

[RegisterTypeInIl2Cpp]
public class DynamicTreeData : MonoBehaviour
{
	#region Constants
	public float clearAccumulation { get; } = 0; //0cm
	public float lowestAccumulation { get; } = 5; //5cm
	public float lowAccumulation { get; } = 15; //10cm
	public float lowMediumAccumulation { get; } = 40; //15cm
	public float mediumAccumulation { get; } = 65; //25cm
	public float mediumHighAccumulation { get; } = 80; //35cm
	public float highAccumulation { get; } = 85; //45cm
	public float highestAccumulation { get; } = 95; //50cm
	public float fullAccumulation { get; } = 100; //55-60cm
	#endregion

	public bool hasInstancedTrees;
	public enum state { Clear, Lowest, Low, LowMed, Med, MedHigh, High, Highest, Full };

	public state currentState;

	public float currentAccumulation { get; set; } = 0;
	public float accumulationAmountPerHour { get; set; } = 0;

	public DynamicTreeSaveDataProxy? SaveDataProxy = new();

	public async void Start()
	{
		await LoadAndSaveData();
	}
	public async void Update()
	{
		await Accumulate(GameManager.GetTimeOfDayComponent().GetTODHours(Time.deltaTime));

		state prevState = currentState;
		currentState = SetState();

		if (HitThreshold(prevState))
		{
			TextureHelper.ReplaceTreeTextures(GameManager.m_ActiveScene, hasInstancedTrees);
		}

	}

	public async Task Accumulate(float numHoursDelta)
	{
		accumulationAmountPerHour = await CalculateAccumulationAmountPerHour();
		currentAccumulation += accumulationAmountPerHour * numHoursDelta;

		currentAccumulation = Mathf.Clamp(currentAccumulation, 0f, 100f);
	}

	public async Task<float> CalculateAccumulationAmountPerHour()
	{
		float accumulationAmount = 0;

		switch (GameManager.GetWeatherComponent().GetWeatherStage())
		{
			case WeatherStage.DenseFog:
			case WeatherStage.PartlyCloudy:
			case WeatherStage.Clear:
			case WeatherStage.Cloudy:
			case WeatherStage.LightFog:
			case WeatherStage.ClearAurora:
			case WeatherStage.ElectrostaticFog:
				accumulationAmount = 0;
				if (currentAccumulation >= 80f) accumulationAmount = -10f;

				switch (GameManager.GetWindComponent().m_CurrentStrength)
				{
					case WindStrength.SlightlyWindy:
						accumulationAmount -= 2f;
						break;
					case WindStrength.Windy:
						accumulationAmount -= 5f;
						break;
					case WindStrength.VeryWindy:
						accumulationAmount -= 10f;
						break;
					case WindStrength.Blizzard:
						accumulationAmount -= 20f;
						break;
					default:
						break;
				}

				break;
			case WeatherStage.LightSnow:
				accumulationAmount = 3f;
				switch (GameManager.GetWindComponent().m_CurrentStrength)
				{
					case WindStrength.SlightlyWindy:
						accumulationAmount += 1f;
						break;
					case WindStrength.Windy:
						accumulationAmount += 3f;
						break;
					case WindStrength.VeryWindy:
						accumulationAmount += 5f;
						break;
					case WindStrength.Blizzard:
						accumulationAmount += 7f;
						break;
					default:
						break;
				}
				break;
			case WeatherStage.HeavySnow:
				accumulationAmount = 7f;
				switch (GameManager.GetWindComponent().m_CurrentStrength)
				{
					case WindStrength.SlightlyWindy:
						accumulationAmount += 3f;
						break;
					case WindStrength.Windy:
						accumulationAmount += 6f;
						break;
					case WindStrength.VeryWindy:
						accumulationAmount += 8f;
						break;
					case WindStrength.Blizzard:
						accumulationAmount += 10f;
						break;
					default:
						break;
				}
				break;
			case WeatherStage.Blizzard:
				accumulationAmount = 30f;
				break;
			default:
				break;
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
		else if (GameManager.GetWeatherComponent().GetWeatherStage() == WeatherStage.LightSnow) return lowAccumulation;
		else if (GameManager.GetWeatherComponent().GetWeatherStage() == WeatherStage.HeavySnow) return mediumAccumulation;
		else if (GameManager.GetWeatherComponent().GetWeatherStage() == WeatherStage.Blizzard) return highAccumulation;
		else return clearAccumulation;
	}
	public bool IsNotSnowing()
	{
		if (GameManager.GetWeatherComponent().GetWeatherStage() != WeatherStage.LightSnow && GameManager.GetWeatherComponent().GetWeatherStage() != WeatherStage.HeavySnow && GameManager.GetWeatherComponent().GetWeatherStage() != WeatherStage.Blizzard) return true;
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
	public async Task LoadAndSaveData()
	{
		SaveDataProxy ??= await Main.SaveDataManager?.Load();

		if (SaveDataProxy == null)
		{
			currentAccumulation = GetStartingAccumulation();

			SaveDataProxy = new();
			SaveDataProxy.savedAccumulation = currentAccumulation;
		}
		else
		{
			currentAccumulation = SaveDataProxy.savedAccumulation;
		}
		
		await Main.SaveDataManager.Save(SaveDataProxy);
	}
}
