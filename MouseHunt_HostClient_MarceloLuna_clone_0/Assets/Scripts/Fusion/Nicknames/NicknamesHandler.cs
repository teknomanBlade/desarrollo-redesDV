using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknamesHandler : MonoBehaviour
{
    public static NicknamesHandler Instance { get; private set; }

    [SerializeField] NicknameText _nicknamePrefab;

    private List<NicknameText> _allNicknames;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;

        _allNicknames = new List<NicknameText>();
    }

    public NicknameText AddNickname(PlayerModel owner) 
    {
        var newNickname = Instantiate(_nicknamePrefab, transform)
            .SetOwner(owner);

        _allNicknames.Add(newNickname);

        owner.OnLeft += () =>
        {
            _allNicknames.Remove(newNickname);
            Destroy(newNickname.gameObject);
        };

        return newNickname;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*_allNicknames.ForEach(nickname => 
        {
            nickname.UpdatePosition();
        });*/
    }


}
