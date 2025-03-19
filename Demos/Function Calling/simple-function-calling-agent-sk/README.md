# Simple Function Calling Agent with Semantic Kernel

This project demonstrates how to use Microsoft Semantic Kernel to create a chat agent with function calling capabilities.

## Overview

This application is a console-based chat agent that can:
- Engage in conversation with the user
- Call functions (tools) based on user requests
- Handle two specific functions:
  - `GetEmployeeId`: Get an employee ID based on a name
  - `BookSlot`: Book a meeting slot for an employee

## Key Components

- **Semantic Kernel**: Used for LLM integration and function calling
- **Plugins**: C# classes with methods decorated with `[KernelFunction]` attributes
- **Required Parameters**: Function parameters marked with `[Required]` attribute to ensure they are provided
- **ConsoleWorker**: Manages the chat loop and interaction with the LLM
- **LocalDebuggingProxy**: Optional HTTP proxy for debugging network traffic

## How It Works

1. The application sets up a Semantic Kernel instance with the OpenAI chat completion service
2. It registers plugins containing the functions that can be called
3. The chat loop collects user input and sends it to the LLM
4. When the LLM detects a function call is needed, Semantic Kernel automatically:
   - Identifies the appropriate function
   - Extracts parameters from the user's request
   - Calls the function
   - Returns the result to the LLM
5. The LLM incorporates the function result into its response

## Running the Application

1. Set your OpenAI API key in `appsettings.json` or use user secrets:
   ```
   dotnet user-secrets set "OPENAI_API_KEY" "your-api-key"
   ```

2. Build and run the application:
   ```
   dotnet build
   dotnet run
   ```

3. Interact with the chat agent by typing messages. Try asking it to:
   - Get an employee ID: "What's the employee ID for John Smith?"
   - Book a meeting: "Book a meeting with employee 42 for tomorrow at 2pm for 30 minutes"

## Comparison with Azure.AI.OpenAI Implementation

This implementation uses Semantic Kernel instead of directly using the Azure.AI.OpenAI SDK. Key differences:

1. **Function Definition**:
   - SK: Functions are defined as C# methods with attributes
   - SK: Parameters can be marked as required using the `[Required]` attribute
   - Azure SDK: Functions are defined using JSON schema
   - Azure SDK: Required parameters are specified in a separate "Required" array

2. **Function Invocation**:
   - SK: Automatic function invocation with `ToolCallBehavior.AutoInvokeKernelFunctions`
   - Azure SDK: Manual handling of function calls and responses

3. **Chat History Management**:
   - SK: Uses the `ChatHistory` class
   - Azure SDK: Manually manages message arrays

The Semantic Kernel approach provides a more streamlined, type-safe way to define and use functions with LLMs.
