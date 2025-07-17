using UnityEngine.SceneManagement; // Para manejo de escenas
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Este evento se activa cuando el jugador entra en una zona de victoria.
    /// </summary>
    public class PlayerEnteredVictoryZone : Simulation.Event<PlayerEnteredVictoryZone>
    {
        public VictoryZone victoryZone;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            model.player.animator.SetTrigger("victory");
            model.player.controlEnabled = false;

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            // Verifica si hay un siguiente nivel. Si no, vuelve al título (índice 0)
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                Simulation.Schedule<LoadNextLevel>(0.5f).sceneIndex = nextSceneIndex;
            }
            else
            {
                Simulation.Schedule<LoadNextLevel>(0.5f).sceneIndex = 0; // Pantalla de título
            }
        }

        // Evento auxiliar para cambiar de escena después de un pequeño delay
        public class LoadNextLevel : Simulation.Event<LoadNextLevel>
        {
            public int sceneIndex;

            public override void Execute()
            {
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }
}
