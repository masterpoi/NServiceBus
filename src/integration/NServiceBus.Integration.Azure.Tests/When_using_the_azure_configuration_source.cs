using NBehave.Spec.NUnit;
using NServiceBus.Config.ConfigurationSource;
using NUnit.Framework;
using Rhino.Mocks;

namespace NServiceBus.Integration.Azure.Tests
{
    [TestFixture]
    public class When_using_the_azure_configuration_source
    {
        private IAzureConfigurationSettings azureSettings;
        private IConfigurationSource configSource;

        [SetUp]
        public void SetUp()
        {
            azureSettings = MockRepository.GenerateStub<IAzureConfigurationSettings>();

            configSource = new AzureConfigurationSource(azureSettings);
        }

        [Test]
        public void The_service_configuration_should_override_appconfig()
        {   
            azureSettings.Stub(x => x.GetSetting("TestConfigSection.StringSetting")).Return("test");

            configSource.GetConfiguration<TestConfigSection>().StringSetting.ShouldEqual("test");
        }

        [Test]
        public void Overrides_should_be_possible_for_non_existing_sections()
        {
            azureSettings.Stub(x => x.GetSetting("SectionNotPresentInConfig.SomeSetting")).Return("test");

            configSource.GetConfiguration<SectionNotPresentInConfig>().SomeSetting.ShouldEqual("test");
        }

        [Test]
        public void No_section_should_be_returned_if_both_azure_and_app_confis_are_empty()
        {
            configSource.GetConfiguration<SectionNotPresentInConfig>().ShouldBeNull();
        }

        [Test]
        public void Value_types_should_be_converted_from_string_to_its_native_type()
        {
            azureSettings.Stub(x => x.GetSetting("TestConfigSection.IntSetting")).Return("23");

            configSource.GetConfiguration<TestConfigSection>().IntSetting.ShouldEqual(23);
        }
    }
} 