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
    private float learningStep = 0.01f; // Learning rate for backpropagation

    [SerializeField]
    private int neuronsPerLayer = 8; // Number of neurons in each layer

    [HideInInspector]
    [SerializeField]
    private bool isInitialized = false;

    private Movement movement;

    void Start()
    {

        try
        {
            movement = GetComponent<Movement>();
        }
        catch (System.NullReferenceException e)
        {
            Debug.LogError("Movement component not found: " + e.Message);
        }

        if (isInitialized) return;

        // Initialize the brain, e.g., load weights, biases, etc.
        neuralNet = new Neuron[depth][];
        for (int i = 0; i < depth; i++)
        {
            neuralNet[i] = new Neuron[neuronsPerLayer];
            for (int j = 0; j < neuronsPerLayer; j++)
            {
                // Randomly initialize weights and biases
                float[] weights = new float[neuronsPerLayer];
                for (int k = 0; k < weights.Length; k++)
                {
                    weights[k] = Random.Range(-1f, 1f);
                }

                float bias = Random.Range(-1f, 1f);
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
                weights[j] = Random.Range(-1f, 1f);
            }

            float bias = Random.Range(-1f, 1f);
            outputLayer[i] = new Neuron(weights, bias);
        }

        isInitialized = true;
    }

    public void InitializeForwardPass(float[][] inputOneHot)
    {

        float[] output = new float[neuralNet.Length];
        for (int i = 0; i < neuralNet.Length; i++)
        {


            // Looping over each input vector and the values in the vector
            //Summing the weighted and biased values in the input vector
            //Then summing them all together to get one vector or a value per neuron
            float[] sum = new float[neuronsPerLayer];

            for (int j = 0; j < inputOneHot.Length; j++)
            {
                for (int k = 0; k < inputOneHot[j].Length; k++)
                {
                    // Assuming inputOneHot[k] is a one-hot encoded vector
                    sum[i] += inputOneHot[j][k] * neuralNet[0][i].weights[k];
                }
            }
            sum[i] += neuralNet[0][i].bias;
            output[i] = ReLU(sum[i]);
        }

        OutputResults(ForwardPass(output));
    }

    private float[] ForwardPass(float[] input)
    {
        // We take the each input and pass it through each neuron in the next layer
        // This neuron takes the input and multiplies it by the weight, adds the bias, and applies ReLU activation

        float[][] output = new float[depth][];

        // Looping over each layer and neuron in each layer
        for (int i = 1; i < depth; i++)
        {
            output[i] = new float[neuronsPerLayer];
            for (int j = 0; j < neuronsPerLayer; j++)
            {
                // Looping over each input
                //Summing the weighted and biased values in the input
                //Then summing them all together to get one value per neuron
                float[] sum = new float[neuronsPerLayer];
                for (int k = 0; k < input.Length; k++)
                {
                    sum[j] += input[k] * neuralNet[i][j].weights[k];
                }
                sum[j] += neuralNet[i][j].bias;
                output[i][j] = ReLU(sum[j]);
            }
        }
        return output[depth - 1];
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
            output[i] = ReLU(sum);
        }

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
                    neuralNet[i][j].weights[k] += Random.Range(-.5f, .5f);
                }

                neuralNet[i][j].bias += Random.Range(-.5f, .5f);
            }
        }


        // Initialize the output layer
        for (int i = 0; i < outputLayer.Length; i++)
        {
            // Randomly initialize weights and biases for the output layer
            for (int j = 0; j < outputLayer[i].weights.Length; j++)
            {
                outputLayer[i].weights[j] += Random.Range(-.5f, .5f);   
            }

            outputLayer[i].bias +=  Random.Range(-.5f, .5f);
        }
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
