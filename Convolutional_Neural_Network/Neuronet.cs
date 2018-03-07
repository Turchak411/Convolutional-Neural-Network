﻿using System.Collections.Generic;

namespace Convolutional_Neural_Network
{
    public class Neuronet
    {
        private List<Layer> m_layerList = new List<Layer>();

        private Neuronet() { }

        public Neuronet(int[] neuronsNumberByLayers, int receptorsNumber, FileManager fileManager)
        {
            Layer firstLayer = new Layer(neuronsNumberByLayers[0], receptorsNumber, 0, fileManager);
            m_layerList.Add(firstLayer);

            for (int i = 1; i < neuronsNumberByLayers.Length; i++)
            {
                Layer layer = new Layer(neuronsNumberByLayers[i], neuronsNumberByLayers[i - 1], i, fileManager);
                m_layerList.Add(layer);
            }
        }

        public double[] Handle(double[] data)
        {
            double[] tempData = data;

            for (int i = 0; i < m_layerList.Count; i++)
            {
                tempData = m_layerList[i].Handle(tempData);
            }

            // There is one double value at the last handle

            return tempData;
        }

        public void Teach(double[] data, double[] rightAnwsersSet, double learnSpeed)
        {
            // Подсчет ошибки:
            m_layerList[m_layerList.Count - 1].CalcErrorAsOut(rightAnwsersSet);

            for (int i = m_layerList.Count - 2; i >= 0; i--)
            {
                double[][] nextLayerWeights = m_layerList[i + 1].GetWeights();
                double[] nextLayerErrors = m_layerList[i + 1].GetErrors();

                m_layerList[i].CalcErrorAsHidden(nextLayerWeights, nextLayerErrors);
            }

            // Корректировка весов нейронов:
            double[] anwsersFromPrewLayer = data;

            for (int i = 0; i < m_layerList.Count; i++)
            {
                m_layerList[i].ChangeWeights(learnSpeed, anwsersFromPrewLayer);
                anwsersFromPrewLayer = m_layerList[i].GetLastAnwsers();
            }
        }
    }
}
