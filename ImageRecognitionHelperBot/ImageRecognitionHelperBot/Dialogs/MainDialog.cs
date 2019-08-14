// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.5.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

using ImageRecognitionHelperBot;
using ImageRecognitionHelperBot.CognitiveModels;
using System.IO;
using Microsoft.Bot.Connector;

namespace ImageRecognitionHelperBot.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly FlightBookingRecognizer _luisRecognizer;
        protected readonly ILogger Logger;

        string contractId = "";


        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(FlightBookingRecognizer luisRecognizer, RegistrationDialog registerDialog, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _luisRecognizer = luisRecognizer;
            Logger = logger;

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(registerDialog);
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                CheckRegistrationStepAsync,
                ShowImageStepAsync,
                SendResultToServerAsync
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);

        }

        private async Task<DialogTurnResult> CheckRegistrationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //if (string.IsNullOrEmpty(contractId))
            //{
            //    return await stepContext.BeginDialogAsync(nameof(RegistrationDialog), cancellationToken);
            //}
            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> ShowImageStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // get image from api - let's assume that this image we got from api
            // show image to user
            var response = MessageFactory.Attachment(GetInlineAttachment());
            await stepContext.Context.SendActivityAsync(response, cancellationToken);

            var messageText = "What does this say?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> SendResultToServerAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // get the user's input
            // send to the server
            // restart dialog 
            return await stepContext.ReplaceDialogAsync(InitialDialogId, cancellationToken);

        }

        private static Attachment GetInlineAttachment()
        {
            var imagePath = Path.Combine(Environment.CurrentDirectory, @"Resources\sample.JPG");
            var imageData = Convert.ToBase64String(File.ReadAllBytes(imagePath));

            return new Attachment
            {
                Name = @"Resources\sample.JPG",
                ContentType = "image/jpg",
                ContentUrl = $"data:image/jpg;base64,{imageData}"
            };
        }
    }
}
