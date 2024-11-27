using System.Collections.Generic;
using UnityEngine;

namespace IA
{
    [RequireComponent(typeof(Renderer))]
    [ExecuteAlways]
    public class RGBWLight2 : DMXFixture
    {
        public override int getUniverse { get { return universe; } }
        public override int getDmxAddress { get { return dmxAddress; } }
        public override Dictionary<string, int> getChannelFunctions { get { return channelFunctions; } }

        private Dictionary<string, int> channelFunctions = new Dictionary<string, int> 
        { 
            { ChannelName.RED, 0 }, 
            { ChannelName.GREEN, 1 }, 
            { ChannelName.BLUE, 2 }, 
            { ChannelName.WHITE, 3 } 
        };

        private Material emissiveMaterial;

        void GetWireData()
        {
            if (artNetData.dmxDataMap != null && emissiveMaterial != null)
            {
                // Calculer la couleur d'émission
                var color = new Color(
                    artNetData.dmxDataMap[universe - 1][dmxAddress - 1 + (int)channelFunctions[ChannelName.RED]] / 256f,
                    artNetData.dmxDataMap[universe - 1][dmxAddress - 1 + (int)channelFunctions[ChannelName.GREEN]] / 256f,
                    artNetData.dmxDataMap[universe - 1][dmxAddress - 1 + (int)channelFunctions[ChannelName.BLUE]] / 256f
                );

                // Ajouter le blanc
                color += Color.white * 0.5f * artNetData.dmxDataMap[universe - 1][dmxAddress - 1 + (int)channelFunctions[ChannelName.WHITE]] / 256f;

                // Appliquer la couleur émissive
                emissiveMaterial.SetColor("_EmissionColor", color);
            }
        }

        void Update()
        {
            GetWireData();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            // Obtenir le Renderer et le matériau
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                emissiveMaterial = renderer.material; // Matériau unique pour chaque instance
            }

            FindDataMap();
            artNetData.dmxUpdate.AddListener(UpdateDMX);
        }

        void UpdateDMX()
        {
            GetWireData();
        }
    }
}
