using System;
using System.Collections.Generic;
using UnityEngine;

public class User {
    const string NAME_KEY = "username";
    const string DEFAULT_NAME = "new_user";
    
    public struct NameValidation {
        public Predicate<string> isPassed;
        public string errorMsg;
    }

    static readonly List<NameValidation> nameValidations =
        new List<NameValidation> {
            new NameValidation {
                isPassed = name => name.Length >= 6,
                errorMsg = "Name must be atleast 6 characters"
            },
            new NameValidation {
                isPassed = name => name.Length <= 24,
                errorMsg = "Name can't be more than 24 characters"
            },
        };    

    public static bool IsValidName(string name) {
        return !nameValidations.Exists(
            validation => !validation.isPassed(name)
        );
    }    

    public static List<NameValidation> FailedValidations(string name) {
        return nameValidations.FindAll(validation =>
            !validation.isPassed(name)
        );
    }

    public static bool Exists() {
        return PlayerPrefs.HasKey(NAME_KEY);
    }

    public static void New(string name) {
        PlayerPrefs.SetString(NAME_KEY, name);
    }

    public static string GetName() {
        return Exists()
               ? PlayerPrefs.GetString(NAME_KEY)
               : DEFAULT_NAME;
    }
}
