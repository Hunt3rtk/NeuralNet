using System;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public class Neuron
    {
        public float[] weights;
        public float bias;

        public Neuron(float[] weights, float bias)
        {
            this.weights = weights;
            this.bias = bias;
        }
    }

    public float ReLU(float a) => a > 0 ? a : 0;

    public double ReLUDerivative(float a) => a > 0 ? 1.0 : 0.0;

    [HideInInspector]
    [SerializeField]
    private Neuron[][] neuralNet;

    [HideInInspector]
    [SerializeField]
    private Neuron[] outputLayer = new Neuron[2]; // Assuming 2 outputs for movement direction

    [SerializeField]
    private int depth = 3; // Number of layers in the neural network

    [SerializeField]
    private float mutationVariation = 0.1f;

    [SerializeField]
    private int neuronsPerLayer = 5; // Number of neurons in each layer

    [SerializeField]
    int inputSize;

    [HideInInspector]
    [SerializeField]
    private bool isInitialized = false;

    private Movement movement;

    private Vision vision;

    private Vector3 initialposition;

    void Start()
    {

        initialposition = transform.position;

        try
        {
            movement = GetComponent<Movement>();
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogError("Movement component not found: " + e.Message);
        }

        try
        {
            vision = GetComponent<Vision>();
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogError("Movement component not found: " + e.Message);
        }

        if (vision != null)
        {
            inputSize = vision.GetInputSize();
        }
        else
        {
            Debug.LogError("Vision component not found. Cannot determine input size.");
            inputSize = 0; // Default to 0 or some safe value
        }



        if (isInitialized) return;

        // Initialize the brain, e.g., load weights, biases, etc.
        neuralNet = new Neuron[depth][];
        for (int i = 0; i < depth; i++)
        {
            neuralNet[i] = new Neuron[neuronsPerLayer];
            for (int j = 0; j < neuronsPerLayer; j++)
            {

                int prevLayerSize = (i == 0) ? inputSize : neuronsPerLayer;

                // Randomly initialize weights and biases
                float[] weights = new float[prevLayerSize];
                for (int k = 0; k < weights.Length; k++)
                {
                    weights[k] = UnityEngine.Random.Range(-1f, 1f);
                }

                float bias = UnityEngine.Random.Range(-.5f, .5f);
                neuralNet[i][j] = new Neuron(weights, bias);
            }
        }


        // Initialize the output layer
        for (int i = 0; i < outputLayer.Length; i++)
        {
            // Randomly initialize weights and biases for the output layer
            float[] weights = new float[neuronsPerLayer];
            for (int j = 0; j < weights.Length; j++)
            {
                weights[j] = UnityEngine.Random.Range(-1f, 1f);
            }

            float bias = UnityEngine.Random.Range(-.5f, .5f);
            outputLayer[i] = new Neuron(weights, bias);
        }

        isInitialized = true;
    }

    public void InitializeForwardPass(float[][] inputOneHot)
    {

        if (isInitialized == false) return;

        float[] output = new float[neuronsPerLayer];
        for (int i = 0; i < neuronsPerLayer; i++)
        {
            // Looping over each input
            float[] sum = new float[neuronsPerLayer];
            for (int k = 0; k < inputOneHot[i].Length; k++)
            {
                // Assuming inputOneHot[k] is a one-hot encoded vector
                sum[i] += inputOneHot[i][k] * neuralNet[0][i].weights[k];
            }
            sum[i] += neuralNet[0][i].bias;
            output[i] = ReLU(sum[i]);
        }

        OutputResults(ForwardPass(output));
    }

    private float[] ForwardPass(float[] input, byte depthCount = 1)
    {
        // We take the each input and pass it through each neuron in the next layer
        // This neuron takes the input and multiplies it by the weight, adds the bias, and applies ReLU activation

        // Looping over each layer and neuron in each layer
        if (depthCount >= depth) return input;

        float[] output = new float[neuronsPerLayer];

        for (int j = 0; j < neuronsPerLayer; j++)
        {
            // Looping over each input
            //Summing the weighted and biased values in the input
            //Then summing them all together to get one value per neuron
            float[] sum = new float[neuronsPerLayer];
            for (int k = 0; k < input.Length; k++)
            {
                sum[j] += input[k] * neuralNet[depthCount][j].weights[k];
            }
            sum[j] += neuralNet[depthCount][j].bias;
            output[j] = ReLU(sum[j]);
        }
        return ForwardPass(output, ++depthCount);
    }

    private void OutputResults(float[] lastInput)
    {
        float[] output = new float[outputLayer.Length];

        for (int i = 0; i < outputLayer.Length; i++)
        {
            float sum = 0;
            for (int j = 0; j < lastInput.Length; j++)
            {
                sum += lastInput[j] * outputLayer[i].weights[j];
            }
            sum += outputLayer[i].bias;
            output[i] = sum;
        }

        if (output[0] <= .3f && output[0] >= -.3f) output[0] = 0;
        if (output[1] <= .3f && output[1] >= -.3f) output[1] = 0;

        // Process the output from the neural network
        // For example, you can use the output to control movement or other actions
        if (movement != null)
        {
            Vector2 direction = new Vector2(output[0], output[1]);
            movement.Move(direction);
        }
        else
        {
            Debug.LogWarning("Movement component is not assigned.");
        }
    }

    public void Mutate()
    {  
        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < neuronsPerLayer; j++)
            {
                // Randomly initialize weights and biases
                for (int k = 0; k < neuralNet[i][j].weights.Length; k++)
                {
                    neuralNet[i][j].weights[k] += UnityEngine.Random.Range(-mutationVariation, mutationVariation);
                }

                neuralNet[i][j].bias += UnityEngine.Random.Range(-mutationVariation, mutationVariation);
            }
        }


        // Initialize the output layer
        for (int i = 0; i < outputLayer.Length; i++)
        {
            // Randomly initialize weights and biases for the output layer
            for (int j = 0; j < outputLayer[i].weights.Length; j++)
            {
                outputLayer[i].weights[j] += UnityEngine.Random.Range(-mutationVariation, mutationVariation);   
            }

            outputLayer[i].bias +=  UnityEngine.Random.Range(-mutationVariation, mutationVariation);
        }
    }

    public float GetDistanceFromStart()
    {
        return Vector3.Distance(initialposition, transform.position);
    }

    public Neuron[][] GetNeuralNet()
    {
        return neuralNet;
    }

    public Neuron[][] SetNeuralNet(Neuron[][] newNet)
    {
        neuralNet = newNet;
        return neuralNet;
    }

    public Neuron[] GetOutputLayer()
    {
        return outputLayer;
    }

    public Neuron[] SetOutputLayer(Neuron[] newLayer)
    {
        outputLayer = newLayer;
        return outputLayer;
    }
}
