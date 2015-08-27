using UnityEngine;
using UnityEngine.UI;

public class NewUserMenu : MonoBehaviour {
    [SerializeField] Button doneButton;
    [SerializeField] InputField nameField;
    [SerializeField] Text errorMsg;
    [SerializeField] Animator animator;

    void Start() {
        CheckShow();
    }

    void CheckShow() {
        if (!User.Exists())
            Show();
    }

    void Show() {
        doneButton.onClick.AddListener(DoneButtonOnClick);
		animator.SetBool("isHidden", false);
    }

    void DoneButtonOnClick() {
        if (User.IsValidName(nameField.text))
            RegisterNewUser();
        else
            ShowErrors();
    }

    void RegisterNewUser() {
        User.New(nameField.text);
		animator.SetBool("isHidden", true);
    }

    void ShowErrors() {
        errorMsg.text = "";

        User.FailedValidations(nameField.text).ForEach(failedValidation =>
            errorMsg.text += failedValidation.errorMsg + "\n"
        );
    }
}
