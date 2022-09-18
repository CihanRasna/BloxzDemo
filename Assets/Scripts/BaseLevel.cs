using System;
using Managers;
using UnityEngine;

public abstract class BaseLevel : MonoBehaviour
{
    public interface ILevelDelegate
    {
        void Level_DidLoad(BaseLevel baseLevel);
        void Level_DidStart(BaseLevel baseLevel);
        void Level_DidSuccess(BaseLevel baseLevel, float score);
        void Level_DidFail(BaseLevel baseLevel);
    }

    public ILevelDelegate listener;

    #region Status

    public enum State
    {
        Loading,
        Loaded,
        Started,
        Succeeded,
        Failed
    }

    public State state { protected set; get; }

    #endregion

    #region States

    public void LevelLoaded()
    {
        ValidateLevel();

        if (state == State.Loading)
        {
            InputManager.Instance.isFirstTouch = false;
            state = State.Loaded;
            LevelDidLoad();
            listener.Level_DidLoad(this);
        }
    }

    protected virtual void StartLevel()
    {
        if (state == State.Loaded)
        {
            state = State.Started;
            LevelDidStart();
            listener.Level_DidStart(this);
        }
    }

    protected virtual void Success(float score)
    {
        if (state != State.Succeeded && state != State.Failed)
        {
            PersistManager.Instance.playedLevelCount += 1;
            state = State.Succeeded;
            listener.Level_DidSuccess(this, score);
        }
    }

    protected virtual void Fail()
    {
        if (state != State.Failed && state != State.Succeeded)
        {
            state = State.Failed;
            listener.Level_DidFail(this);
        }
    }

    #endregion

    #region Events

    protected virtual void LevelDidLoad()
    {
    }

    protected virtual void LevelDidStart()
    {
    }

    public void StartOnFirstTouch()
    {
        StartLevel();
    }

    #endregion

    public Player player;

    #region Validation

    private void  ValidateLevel()
    {
        if (GetComponentsInChildren<Player>().Length > 1)
        {
            throw new Exception("There are more than one PlayerMovementController component in the level!");
        }
    }

    #endregion
}