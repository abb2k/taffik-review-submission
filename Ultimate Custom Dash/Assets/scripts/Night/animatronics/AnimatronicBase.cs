using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class AnimatronicBase : MonoBehaviour
{
    [BoxGroup("Base Settings")]
    public string Name = "AnimatronicName";
    [BoxGroup("Base Settings")]
    public int AILevel;
    [BoxGroup("Base Settings")]
    public float OppretunityEvery = 5;
    [BoxGroup("Base Settings")]
    [ReadOnly]public float OppretunityTimer;
    [BoxGroup("Base Settings")]
    public int ExtraChance;
    [Space]
    [BoxGroup("Base Settings")]
    public AnimationClip Animation;
    [BoxGroup("Base Settings")]
    public AudioClip JumpscareScream;
    [BoxGroup("Base Settings")]
    public bool IsLethal = true;
    [BoxGroup("Base Settings")]
    public bool TakeDownCams = true;
    [BoxGroup("Base Settings")]
    public bool TakeDownMask = true;
    [BoxGroup("Base Settings")]
    public AudioClip[] Voicelines = new AudioClip[1];
    [Space]
    [BoxGroup("Base Settings")]
    [ReadOnly] public NightManager NM;
    [BoxGroup("Base Settings")]
    [ReadOnly] public GameManager GM;
    bool testbool;
    [BoxGroup("Base Settings")]
    public CustomValues customVals = new CustomValues();

    [HideInInspector] 
    public float timerMultiplier = 1;

    [HideInInspector]
    public bool attackState = false;

    [HideInInspector]
    public bool oneTimeAttackStateLock = false;

    void Start()
    {
        OppretunityTimer = OppretunityEvery;
        NM = NightManager.inctance;
        GM = GameManager.get();

        AddCustomValue(new IntValue(AILevel, "AILevel"));
        AddCustomValue(new FloatValue(OppretunityEvery, "Oppretunity"));
        AddCustomValue(new IntValue(ExtraChance, "ExtraChance"));
        AddCustomValue(new BoolValue(IsLethal, "IsLethal"));
        AddCustomValue(new BoolValue(TakeDownCams, "TakeDownCams"));
        AddCustomValue(new BoolValue(TakeDownMask, "TakeDownMask"));

        AnimatronicGameStart();
    }

    void Update()
    {
        if (NM != null)
            if (NM.NightOngoing)
            {
                if (OppretunityTimer > 0)
                {
                    if (!testbool)
                    {
                        testbool = true;
                        OppretunityTimer += Time.deltaTime;
                    }
                    OppretunityTimer -= Time.deltaTime * timerMultiplier;

                    if (OppretunityTimer <= 0)
                    {
                        float overshoot = OppretunityTimer;
                        OppretunityTimer = OppretunityEvery;
                        OppretunityTimer += overshoot;

                        int randomChance = Random.Range(0, 20 + ExtraChance);
                        if (randomChance <= AILevel && AILevel != 0)
                        {
                            OnOppretunity();
                        }
                    }
                }
                else
                {
                    float overshoot = OppretunityTimer;
                    OppretunityTimer = OppretunityEvery;
                    OppretunityTimer += overshoot;

                    int randomChance = Random.Range(0, 20 + ExtraChance);
                    if (randomChance <= AILevel && AILevel != 0)
                    {
                        OnOppretunity();
                    }
                }
            }
        
        AnimatronicUpdate();

        if (attackState && !oneTimeAttackStateLock)
        {
            if (NM.CamsFullyOpened && !GM.silent || NM.BigBlackscreen.color.a == 1 && !GM.silent || GM.silent)
            {
                oneTimeAttackStateLock = true;
                OnAttackStateJumpscareCall();
            }
        }
    }

    public virtual void OnOppretunity() { }
    public virtual void OnDeathcoined() 
    {
        AILevel = 0;
    }
    public virtual void OnMirrorSummon(int AIChosen) 
    {
        AILevel = AIChosen;
    }

    public virtual void AnimatronicStart() { }
    public virtual void AnimatronicUpdate() { }

    public virtual void AnimatronicGameStart() { }

    public virtual void OnPlayerDied() { }

    public ductPath CreateDuctPathObject()
    {
        ductPath p = null;
        if (GM != null)
        {
            GameObject curr = Instantiate(GM.emptyObject, NM.DuctIconCont);
            p = curr.AddComponent<ductPath>();
            curr.AddComponent<Image>();
            p.leftDuctStart = NM.leftTopPoint;
            p.rightDuctStart = NM.rightTopPoint;
            p.resetPositioin();
            curr.AddComponent<animatronicRelated>().animatronic = this;
        }

        return p;
    }

    public ventPath CreateVentPathObject()
    {
        ventPath p = null;
        if (GM != null)
        {
            GameObject curr = Instantiate(GM.emptyObject, NM.VentIconCont);
            p = curr.AddComponent<ventPath>();
            curr.AddComponent<Image>();
            p.resetPosition();
            curr.AddComponent<animatronicRelated>().animatronic = this;
        }

        return p;
    }

    public void Blocked()
    {
        GM.soundManager.CreateSoundEffect("blocked", GM.soundManager.GetSoundFromList("block"));
        NM.addFazCoins(1);
        attackState = false;
    }

    public virtual GameObject Jumpscare()
    {
        return NM.Jumpscare(Animation, JumpscareScream, IsLethal, TakeDownCams, TakeDownMask, Voicelines, Name);
    }

    public virtual GameObject PlayPoof()
    {
        return Instantiate(GM.poof);
    }

    public void AddCustomValue(IntValue intVal)
    {
        customVals.Ints.Add(intVal);
    }
    public void AddCustomValue(FloatValue floatVal)
    {
        customVals.Floats.Add(floatVal);
    }
    public void AddCustomValue(BoolValue boolVal)
    {
        customVals.Bools.Add(boolVal);
    }
    public void AddCustomValue(EnumValue enumVal)
    {
        customVals.Enums.Add(enumVal);
    }

    public virtual void SetCustomValue(IntValue value) 
    { 
        if (value.keyName == "AILevel")
        {
            AILevel = value.value;
        }
        else if (value.keyName == "ExtraChance")
        {
            ExtraChance = value.value;
        }
        AnimatronicStart();
    }
    public virtual void SetCustomValue(FloatValue value) 
    {
        if (value.keyName == "Oppretunity")
        {
            OppretunityEvery = value.value;
        }
        AnimatronicStart();
    }

    public virtual void SetCustomValue(BoolValue value) 
    {
        if (value.keyName == "IsLethal")
        {
            IsLethal = value.value;
        }
        if (value.keyName == "TakeDownCams")
        {
            TakeDownCams = value.value;
        }
        if (value.keyName == "TakeDownMask")
        {
            TakeDownMask = value.value;
        }
    }
    public virtual void SetCustomValue(EnumValue value) { }

    public virtual void OnNightComplete() { }

    public virtual void OnAttackStateJumpscareCall() 
    {
        Jumpscare();
    }
}
