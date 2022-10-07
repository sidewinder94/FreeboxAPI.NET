using System;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Freebox.Certificates;

/// <summary>
/// Class used to help in validating the Freebox signed certificates
/// </summary>
public static class CertificateHelper
{
    private static X509Certificate2 _rootCA;

    private static X509Certificate2 _eccRootCA;


    private static X509Certificate2 GetFreeboxRootCA()
    {
        if (_rootCA != null) return _rootCA;
            
        var result = new X509Certificate2(@".\Certificates\FreeboxRootCA.pem");

        _rootCA = result;

        return result;
    }

    private static X509Certificate2 GetFreeboxECCRootCA()
    {
        if (_eccRootCA != null) return _eccRootCA;
            
        var result = new X509Certificate2(@".\Certificates\FreeboxECCRootCA.pem");

        _eccRootCA = result;

        return result;
    }

    /// <summary>
    /// Exposes the known Free root certificates
    /// </summary>
    /// <returns>A collection of known root certificates</returns>
    public static X509Certificate2Collection GetFreeboxRootCertificates()
    {
        var result = new X509Certificate2Collection
        {
            GetFreeboxECCRootCA(),
            GetFreeboxRootCA()
        };

        return result;
    }

    /// <summary>
    /// Method used to validate the certificate chain, needed because the root freebox certificate is not trusted by default
    /// </summary>
    /// <param name="request">The request needing the validation request (unused, need the parameter so the signature conforms to <see cref="HttpClientHandler.ServerCertificateCustomValidationCallback"/>)</param>
    /// <param name="certificate">Certificate presented by the server</param>
    /// <param name="certificateChain">Certificate chain attached to the server certificate</param>
    /// <param name="policy">The error(s) detected during the default validation</param>
    /// <returns>True if the certificate is considered valid, false otherwise</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="certificateChain"/> is null</exception>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="System.Security.Cryptography.CryptographicException"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Supprimer le paramètre inutilisé", Justification = "Method signature imposed")]
    public static bool ValidateCertificate(HttpRequestMessage request, X509Certificate2 certificate, X509Chain certificateChain, SslPolicyErrors policy)
    {
        if(certificateChain == null)
        {
            throw new ArgumentNullException(nameof(certificateChain));
        }

        //The root certificate is not included in the response
        if(policy == SslPolicyErrors.RemoteCertificateChainErrors)
        {
            //We include the known root certificates
            certificateChain.ChainPolicy.ExtraStore.Add(GetFreeboxECCRootCA());
            certificateChain.ChainPolicy.ExtraStore.Add(GetFreeboxRootCA());
                
            //We try to build the chain again to take the new roots into account
            if(certificateChain.Build(certificate))
            {
                //If the user machine already trusts the root certificates
                return true;
            }
        }

        // Used when the root certificate is present in the chain but untrusted by the system
        foreach (var element in certificateChain.ChainElements)
        {
            if (element.ChainElementStatus.Any())
            {
                foreach (var status in element.ChainElementStatus)
                {
                    // We only change the behaviour for the untrusted root error, any other error will result in a fail.
                    if (status.Status == X509ChainStatusFlags.UntrustedRoot)
                    {
                        // Check that the root certificate matches one of the valid root certificates
                        if (GetFreeboxRootCertificates().OfType<X509Certificate2>().Any(cert => cert.RawData.SequenceEqual(element.Certificate.RawData)))
                            continue; // Process the next status
                    }

                    return false;
                }
            }
        }

        return true;
    }
}