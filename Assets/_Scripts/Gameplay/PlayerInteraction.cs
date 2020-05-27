namespace cpp.Sen.Gameplay
{
    using UnityEngine;
    using Zenject;

    public sealed class PlayerInteraction : MonoBehaviour
    {
        void Update()
        {
            //todo: add NPCTurn and PlayerTurn modes 
            if(_GameMode.Mode != GameModeController.GameMode.Game) { return; }

            //todo: add input handler class instead
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 3f)) //todo: add interaction settings
                {
                    var interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable == null) { return; }
                    interactable.Interact();
                }
            }
        }

        [Inject] private GameModeController _GameMode;
    }
}
