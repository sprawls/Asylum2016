using UnityEngine;
using System.Collections;

public class OnPictureAppearDissapear : OnPicture {

    public GameObject ObjectToDestroy;
    public GameObject ObjectToAppear;
    public GameObject ObjectToAppearOnPhoto;

    public bool DestroyObjectToAppearOnPhotoAfter = true;

    public override void OnBeforePictureTaken() {
        if (ObjectToAppear!=null) ObjectToAppear.SetActive(true);
        if (ObjectToDestroy != null) Destroy(ObjectToDestroy);
    }

    public override void OnPictureTaken()  {
        if (ObjectToAppearOnPhoto != null) {
            if (DestroyObjectToAppearOnPhotoAfter) Destroy(ObjectToAppearOnPhoto);
            else ObjectToAppearOnPhoto.SetActive(false);
        }
        Unsubscribe();
    }
}
