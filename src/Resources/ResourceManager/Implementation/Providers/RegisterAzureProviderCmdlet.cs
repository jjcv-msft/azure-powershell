﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;

namespace Microsoft.Azure.Commands.ResourceManager.Cmdlets.Implementation
{
    using Microsoft.Azure.Commands.ResourceManager.Cmdlets.SdkModels;
    using Microsoft.Azure.Management.ResourceManager.Models;
    using System.Management.Automation;
    using ProjectResources = Microsoft.Azure.Commands.ResourceManager.Cmdlets.Properties.Resources;

    /// <summary>
    /// Register the previewed features of a certain azure resource provider.
    /// </summary>
    [Cmdlet("Register", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ResourceProvider", SupportsShouldProcess = true), OutputType(typeof(PSResourceProvider))]
    public class RegisterAzureProviderCmdlet : ResourceManagerCmdletBaseWithApiVersion
    {
        /// <summary>
        /// Gets or sets the provider namespace
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The resource provider namespace.")]
        [ValidateNotNullOrEmpty]
        public string ProviderNamespace { get; set; }

        /// <summary>
        /// Gets or sets the consent to permissions flag.
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "A value indicating whether permissions are consented or not.")]
        public bool ConsentToPermissions { get; set; }

        /// <summary>
        /// Executes the cmdlet
        /// </summary>
        protected override void OnProcessRecord()
        {
            ProviderRegistrationRequest providerRegistrationRequest = default;

            if (ConsentToPermissions)
            {
                providerRegistrationRequest = new ProviderRegistrationRequest(new ProviderConsentDefinition(this.ConsentToPermissions));
            }

            this.ConfirmAction(
                processMessage: ProjectResources.RegisterProviderMessage,
                target: this.ProviderNamespace,
                action: () => this.WriteObject(this.ResourceManagerSdkClient.RegisterProvider(providerName: this.ProviderNamespace, providerRegistrationRequest: providerRegistrationRequest)));
        }
    }
}
