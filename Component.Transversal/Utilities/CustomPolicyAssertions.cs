using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Component.Transversal.Utilities
{
    class CustomPolicyAssertions : SecurityPolicyAssertion
    {
        TokenProvider<X509SecurityToken> serviceX509TokenProviderValue;
        TokenProvider<X509SecurityToken> clientX509TokenProviderValue;

        public TokenProvider<X509SecurityToken> ClientX509TokenProvider
        {
            get
            {
                return clientX509TokenProviderValue;
            }
            set
            {
                clientX509TokenProviderValue = value;
            }
        }

        public TokenProvider<X509SecurityToken> ServiceX509TokenProvider
        {
            get
            {
                return serviceX509TokenProviderValue;
            }
            set
            {
                serviceX509TokenProviderValue = value;
            }
        }

        public CustomPolicyAssertions()
            : base()
        {
        }

        public override SoapFilter CreateClientInputFilter(FilterCreationContext context)
        {
            return new CustomSecurityClientInputFilter(this);
        }

        public override SoapFilter CreateClientOutputFilter(FilterCreationContext context)
        {
            return new CustomSecurityClientOutputFilter(this);
        }

        public override SoapFilter CreateServiceInputFilter(FilterCreationContext context)
        {
            return new CustomSecurityServerInputFilter(this);
        }

        public override SoapFilter CreateServiceOutputFilter(FilterCreationContext context)
        {
            return new CustomSecurityServerOutputFilter(this);
        }

        public override void ReadXml(XmlReader reader, IDictionary<string, Type> extensions)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            if (extensions == null)
                throw new ArgumentNullException("extensions");

            bool isEmpty = reader.IsEmptyElement;
            base.ReadAttributes(reader);
            reader.ReadStartElement("CustomPolicyAssertions");

            if (!isEmpty)
            {
                if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "clientToken")
                {
                    reader.ReadStartElement();
                    reader.MoveToContent();
                    Type type = extensions[reader.Name];
                    object instance = Activator.CreateInstance(type);

                    if (instance == null)
                        throw new InvalidOperationException(String.Format(System.Globalization.CultureInfo.CurrentCulture, "Unable to instantiate policy extension of type {0}.", type.AssemblyQualifiedName));

                    TokenProvider<X509SecurityToken> clientProvider = instance as TokenProvider<X509SecurityToken>;
                    clientProvider.ReadXml(reader, extensions);
                    this.ClientX509TokenProvider = clientProvider;
                    reader.ReadEndElement();
                }

                if (reader.MoveToContent() == XmlNodeType.Element && reader.Name == "serviceToken")
                {
                    reader.ReadStartElement();
                    reader.MoveToContent();
                    Type type = extensions[reader.Name];
                    object instance = Activator.CreateInstance(type);

                    if (instance == null)
                        throw new InvalidOperationException(String.Format(System.Globalization.CultureInfo.CurrentCulture, "Unable to instantiate policy extension of type {0}.", type.AssemblyQualifiedName));

                    TokenProvider<X509SecurityToken> serviceProvider = instance as TokenProvider<X509SecurityToken>;
                    serviceProvider.ReadXml(reader, extensions);
                    this.ServiceX509TokenProvider = serviceProvider;
                    reader.ReadEndElement();
                }

                base.ReadElements(reader, extensions);
                reader.ReadEndElement();
            }
        }

        public override IEnumerable<KeyValuePair<string, Type>> GetExtensions()
        {
            List<KeyValuePair<string, Type>> extensions = new List<KeyValuePair<string, Type>>();
            extensions.Add(new KeyValuePair<string, Type>("CustomPolicyAssertions", this.GetType()));

            if (serviceX509TokenProviderValue != null)
            {
                IEnumerable<KeyValuePair<string, Type>> innerException = serviceX509TokenProviderValue.GetExtensions();

                if (innerException != null)
                {
                    foreach (KeyValuePair<string, Type> extension in innerException)
                    {
                        extensions.Add(extension);
                    }
                }
            }

            if (clientX509TokenProviderValue != null)
            {
                IEnumerable<KeyValuePair<string, Type>> innerExceptions = clientX509TokenProviderValue.GetExtensions();

                if (innerExceptions != null)
                {
                    foreach (KeyValuePair<string, Type> extension in innerExceptions)
                    {
                        extensions.Add(extension);
                    }
                }
            }

            return extensions;
        }
    }
}
