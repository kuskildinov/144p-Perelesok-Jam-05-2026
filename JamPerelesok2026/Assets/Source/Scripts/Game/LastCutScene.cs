using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class LastCutScene : MonoBehaviour
{
    public PlayableDirector _playableDirector;
    public CommentsDialogPanel _dialogPanel;
    public PlayerRoot PlayerRoot;
    public LevelRoot LevelRoot;
    public BlackFade BlackFade;
    public DialogPhrase _phrase_1;
    public DialogPhrase _phrase_2;
    public DialogPhrase _phrase_3;
    public GameObject LastPictureObject;
    public CanvasGroup EndListGroup; 

    public void OnStartCutScene()
    {
        PlayerRoot.ToggleActivation(false);
    }

    public void StartFirstPhrase()
    {
        _dialogPanel.Open(_phrase_1);
    }

    public void StartSecondPhrase()
    {
        _dialogPanel.Open(_phrase_2);
    }

    public void StartThirdPhrase()
    {
        _dialogPanel.Open(_phrase_3);
    }

    public void EndLastCutScene()
    {
        BlackFade.FadeOut(1, () =>
        {
            LastPictureObject.gameObject.SetActive(true);
            BlackFade.FadeIn(-1, null);
        });

        StartCoroutine(EndingRoutine());
    }

    private IEnumerator EndingRoutine()
    {
        yield return new WaitForSecondsRealtime(5f);
                       
        EndListGroup.DOFade(1f, 1).WaitForCompletion();

        yield return new WaitForSecondsRealtime(3f);

        LevelRoot.LoadMainMenuScene();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Player>(out Player player))
        {
            _playableDirector.Play();
        }
    }
}
