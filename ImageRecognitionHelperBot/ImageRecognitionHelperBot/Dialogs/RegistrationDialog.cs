// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.5.0

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace ImageRecognitionHelperBot.Dialogs
{
    public class RegistrationDialog : CancelAndHelpDialog
    {
        public RegistrationDialog()
            : base(nameof(RegistrationDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                CheckIfUserSignedContractStepAsync,
                ProcessContractInfoStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> CheckIfUserSignedContractStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Check if user already signed or not (YES/NO question)

            return await stepContext.NextAsync(cancellationToken);
        }

        private async Task<DialogTurnResult> ProcessContractInfoStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Ask user for contract ID

            return await stepContext.NextAsync("Pass contract ID", cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var contractDetails = stepContext.Options;

                return await stepContext.EndDialogAsync(contractDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

      
    }
}
