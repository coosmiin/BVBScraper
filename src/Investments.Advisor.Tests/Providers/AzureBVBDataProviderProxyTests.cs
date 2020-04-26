using Investments.Advisor.AzureProxies;
using Investments.Advisor.Exceptions;
using Investments.Advisor.Tests.Properties;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Investments.Advisor.Tests.Providers
{
	public class AzureBVBDataProviderProxyTests
	{
		[Test]
		public async Task GetBETStocksAsync_ValidResponse_StocksAreCorrect()
		{
			var httpClient = BuildHttpClient(Resources.bvb_index);

			var provider = new AzureBVBDataProviderProxy(httpClient, string.Empty);

			var result = await provider.GetBETStocksAsync();

			Assert.AreEqual(2, result.Length);
			Assert.AreEqual("FP", result[0].Symbol);
			Assert.AreEqual(1.345, result[0].Price);
			Assert.AreEqual(0.2305, result[0].Weight);
		}

		[Test]
		public void GetBETStocksAsync_InvalidCase_ThrowsException()
		{
			var httpClient = BuildHttpClient(Resources.bvb_index_pascal_case);

			var provider = new AzureBVBDataProviderProxy(httpClient, string.Empty);

			Assert.ThrowsAsync<InvalidBETDataException>(async () => await provider.GetBETStocksAsync());
		}

		[Test]
		public void GetBETStocksAsync_EmptyArrayResult_ThrowsException()
		{
			var httpClient = BuildHttpClient("[]");

			var provider = new AzureBVBDataProviderProxy(httpClient, string.Empty);

			Assert.ThrowsAsync<InvalidBETDataException>(async () => await provider.GetBETStocksAsync());
		}

		private HttpClient BuildHttpClient(string testData)
		{
			var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
			httpMessageHandlerMock
				.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(testData)
				});

			var httpClient = new HttpClient(httpMessageHandlerMock.Object);
			httpClient.BaseAddress = new Uri("http://localhost");

			return httpClient;
		}
	}
}
