using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MonsterBehavior : MonoBehaviour
{
    static private float WALL_SOUND_PROBA = .05f;
    static private float SKATE_SOUND_PROBA = .005f;
    static private float BRAKE_SOUND_PROBA = .05f;
    static private float GLIDING_SOUND_PROBA = .025f;
    static private float KEY_SOUND_PROBA = .05f;
    static private float HEARING_PROBABILITY = .4f;

    static private float MODIF_PROBA_AFTER_STOP_ALERTED = .4f;
    static private float NB_SECONDES_MONSTER_ALERTED = 1f;

    public static float riskOfDying = 0f;

    public static float spawnProbability;
    private float probability_valueChanged;
    private bool isAlerted;

    public GameObject monster_Prefab;
    private AudioManager audioManager;

    PostProcessVolume postVolume;
    Vignette vignette;
    

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        postVolume = GameObject.Find("PostProcessVolume").GetComponent<PostProcessVolume>();
        postVolume.profile.TryGetSettings(out vignette);

        spawnProbability = 0f;
        probability_valueChanged = spawnProbability;

        isAlerted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnProbability != probability_valueChanged)
        {
            TryPlaySoundMonster();

            probability_valueChanged = spawnProbability;
        }
        vignette.intensity.value = spawnProbability;
    }

    IEnumerator MonsterIsAlerted()
    {
        //Debug.Log("Monster is alerted");
        isAlerted = true;

        riskOfDying += 0.2f;
        float valueAlerted = spawnProbability;

        yield return new WaitForSeconds(NB_SECONDES_MONSTER_ALERTED);

        if (valueAlerted != spawnProbability)
        {
            TrySpawnMonster();
        }
        spawnProbability *= MODIF_PROBA_AFTER_STOP_ALERTED;
        riskOfDying *= MODIF_PROBA_AFTER_STOP_ALERTED;
        probability_valueChanged = spawnProbability;

        isAlerted = false;
        //Debug.Log("Monster is not alerted");
    }
    public void TrySpawnMonster()
    {
        if (Random.Range(0, 1f) < spawnProbability)
        {
            //Debug.Log("Monster Spawns");

            StartCoroutine(MonsterAttackSounds());

            //  monster_GO = Instantiate(monster_Prefab);

            Player_Movement_Law.isAlive = false;

            EndGameCanvasHandler GameOverCanvas = FindObjectOfType<EndGameCanvasHandler>();
            GameOverCanvas.Loose(2);
        }
    }

    IEnumerator MonsterAttackSounds()
    {
        audioManager.Play(AudioManager.SoundCategory.MonsterPrepareSounds);
        yield return new WaitForSeconds(0.5f);
        audioManager.Play(AudioManager.SoundCategory.MonsterAttacksSounds);
    }

    private void TryPlaySoundMonster()
    {
        if (Random.Range(0, 1f) < spawnProbability)
        {
            //Debug.Log("Monster makes Sound");
            if (spawnProbability < .3f)
                audioManager.Play(AudioManager.SoundCategory.MonsterPassSounds);
            else
                audioManager.Play(AudioManager.SoundCategory.MonsterSounds);

            if (!isAlerted)
                StartCoroutine(MonsterIsAlerted());

        }
    }

    public static void HearingSound_FromWall()
    {
        if (Random.Range(0, 1f) < HEARING_PROBABILITY + spawnProbability)
        {
            //Debug.Log("Hearing sound from wall");
            spawnProbability += WALL_SOUND_PROBA;
        }
    }

    public static void HearingSound_FromSkate()
    {
        if (Random.Range(0, 1f) < HEARING_PROBABILITY + spawnProbability)
        {
            //Debug.Log("Hearing sound from Skate");
            spawnProbability += SKATE_SOUND_PROBA;
        }
    }

    public static void HearingSound_FromGliding()
    {
        if (Random.Range(0, 1f) < HEARING_PROBABILITY + spawnProbability)
        {
            //Debug.Log("Hearing sound from glide");
            spawnProbability += GLIDING_SOUND_PROBA;
        }
    }
    public static void HearingSound_FromBraking()
    {
        if (Random.Range(0, 1f) < HEARING_PROBABILITY + spawnProbability)
        {
            //Debug.Log("Hearing sound from glide");
            spawnProbability += BRAKE_SOUND_PROBA;
        }
    }

    public static void HearingSound_FromKey()
    {
        if (Random.Range(0, 1f) < HEARING_PROBABILITY + spawnProbability)
        {
            //Debug.Log("Hearing sound from key");
            spawnProbability += KEY_SOUND_PROBA;
        }
    }
}
