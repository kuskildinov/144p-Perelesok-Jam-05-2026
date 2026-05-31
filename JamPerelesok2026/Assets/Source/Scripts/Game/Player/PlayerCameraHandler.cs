using UnityEngine;
using Unity.Cinemachine;

public class PlayerCameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _playerModeCam;
    [SerializeField] private CinemachineCamera _characterCam;

    public void Initialize()
    {

    }

    public void OnPlayerModeChanged(PlayerMode mode)
    {
        if(mode == PlayerMode.Character)
        {
            ShowCharacterCam();
        }
        else if(mode == PlayerMode.Player)
        {
            ShowPlayerCam();
        }
    }

    private void ShowPlayerCam()
    {
        _playerModeCam.gameObject.SetActive(true);
        _characterCam.gameObject.SetActive(false);
    }

    private void ShowCharacterCam()
    {
        _playerModeCam.gameObject.SetActive(false);
        _characterCam.gameObject.SetActive(true);
    }
}
