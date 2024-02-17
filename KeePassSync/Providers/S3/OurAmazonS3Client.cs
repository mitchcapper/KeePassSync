using System.Collections.Generic;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime.Internal;
using Amazon.S3.Model.Internal.MarshallTransformations;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;


namespace KeePassSync.Providers.S3 {

	//just used to work around some 3rd party client oddities
	internal class OurAmazonS3Client : AmazonS3Client {
		public OurAmazonS3Client(string awsAccessKeyId, string awsSecretAccessKey, AmazonS3Config clientConfig, List<string> HeadersToStrip = null) : base(awsAccessKeyId, awsSecretAccessKey, clientConfig) {
			if (HeadersToStrip != null)
				RuntimePipeline.AddHandlerBefore<Amazon.S3.Internal.S3Express.S3ExpressPreSigner>(new HeaderStripHandler(HeadersToStrip));


		}

		public class HeaderStripHandler : IPipelineHandler {
			private List<string> headersToStrip;

			public HeaderStripHandler(List<string> headersToStrip) {
				this.headersToStrip = headersToStrip;
			}

			public ILogger Logger { get; set; }
			public IPipelineHandler InnerHandler { get; set; }
			public IPipelineHandler OuterHandler { get; set; }

			public async Task<T> InvokeAsync<T>(IExecutionContext executionContext) where T : AmazonWebServiceResponse, new() {
				RemoveBadHeaders(executionContext);
				if (InnerHandler == null)
					return default(T);
				return await InnerHandler.InvokeAsync<T>(executionContext);
			}

			private void RemoveBadHeaders(IExecutionContext executionContext) {
				if (headersToStrip != null) {
					foreach (var header in headersToStrip)
						executionContext.RequestContext.Request.Headers.Remove(header);
				}
			}

			public void InvokeSync(IExecutionContext executionContext) {
				RemoveBadHeaders(executionContext);
				if (InnerHandler == null)
					return;
				InnerHandler.InvokeSync(executionContext);
			}
		}

		public override CopyObjectResponse CopyObject(CopyObjectRequest request) {
			var invokeOptions = new InvokeOptions();
			invokeOptions.RequestMarshaller = CopyObjectRequestMarshaller.Instance;
			invokeOptions.ResponseUnmarshaller = CopyObjectResponseUnmarshaller.Instance;

			return Invoke<CopyObjectResponse>(request, invokeOptions);
		}

	}

}

