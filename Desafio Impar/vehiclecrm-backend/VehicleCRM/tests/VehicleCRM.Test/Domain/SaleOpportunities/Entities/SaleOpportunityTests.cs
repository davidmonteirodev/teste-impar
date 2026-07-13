using VehicleCRM.Domain.Common.Exceptions;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Enums;

namespace VehicleCRM.Test.Domain.SaleOpportunities.Entities;


public class SaleOpportunityTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateSaleOpportunityWithNewLeadStatus()
    {
        // Arrange
        var customerId = 1L;
        var vehicleId = 2L;
        var proposedValue = 85000m;
        var notes = "Cliente muito interessado";

        // Act
        var saleOpportunity = SaleOpportunity.Create(customerId, vehicleId, proposedValue, notes);

        // Assert
        saleOpportunity.Should().NotBeNull();
        saleOpportunity.CustomerId.Should().Be(customerId);
        saleOpportunity.VehicleId.Should().Be(vehicleId);
        saleOpportunity.ProposedValue.Should().Be(proposedValue);
        saleOpportunity.Notes.Should().Be(notes);
        saleOpportunity.Status.Should().Be(SaleOpportunityStatus.NewLead);
    }

    [Fact]
    public void Create_WithNullNotes_ShouldCreateSaleOpportunity()
    {
        // Arrange & Act
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, null);

        // Assert
        saleOpportunity.Notes.Should().BeNull();
    }

    [Fact]
    public void IsFinalized_WhenStatusIsSold_ShouldReturnTrue()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        saleOpportunity.Update(1L, 2L, SaleOpportunityStatus.Sold, 85000m, "Test");

        // Act
        var result = saleOpportunity.IsFinalized();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsFinalized_WhenStatusIsLost_ShouldReturnTrue()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        saleOpportunity.Update(1L, 2L, SaleOpportunityStatus.Lost, 85000m, "Test");

        // Act
        var result = saleOpportunity.IsFinalized();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsFinalized_WhenStatusIsNotFinalized_ShouldReturnFalse()
    {
        // Arrange & Act & Assert
        var newLead = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        newLead.IsFinalized().Should().BeFalse();

        var inNegotiation = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        inNegotiation.Update(1L, 2L, SaleOpportunityStatus.InNegotiation, 85000m, "Test");
        inNegotiation.IsFinalized().Should().BeFalse();

        var proposalSent = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        proposalSent.Update(1L, 2L, SaleOpportunityStatus.ProposalSent, 85000m, "Test");
        proposalSent.IsFinalized().Should().BeFalse();
    }

    [Fact]
    public void IsInNegotiation_WhenStatusIsInNegotiation_ShouldReturnTrue()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        saleOpportunity.Update(1L, 2L, SaleOpportunityStatus.InNegotiation, 85000m, "Test");

        // Act
        var result = saleOpportunity.IsInNegotiation();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInNegotiation_WhenStatusIsProposalSent_ShouldReturnTrue()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        saleOpportunity.Update(1L, 2L, SaleOpportunityStatus.ProposalSent, 85000m, "Test");

        // Act
        var result = saleOpportunity.IsInNegotiation();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInNegotiation_WhenStatusIsNotInNegotiation_ShouldReturnFalse()
    {
        // Arrange & Act & Assert
        var newLead = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        newLead.IsInNegotiation().Should().BeFalse();

        var sold = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        sold.Update(1L, 2L, SaleOpportunityStatus.Sold, 85000m, "Test");
        sold.IsInNegotiation().Should().BeFalse();

        var lost = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        lost.Update(1L, 2L, SaleOpportunityStatus.Lost, 85000m, "Test");
        lost.IsInNegotiation().Should().BeFalse();
    }

    [Fact]
    public void Update_WhenNotFinalized_ShouldUpdateAllProperties()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Original notes");
        var newCustomerId = 3L;
        var newVehicleId = 4L;
        var newProposedValue = 95000m;
        var newNotes = "Updated notes";
        var newStatus = SaleOpportunityStatus.InNegotiation;

        // Act
        saleOpportunity.Update(newCustomerId, newVehicleId, newStatus, newProposedValue, newNotes);

        // Assert
        saleOpportunity.CustomerId.Should().Be(newCustomerId);
        saleOpportunity.VehicleId.Should().Be(newVehicleId);
        saleOpportunity.Status.Should().Be(newStatus);
        saleOpportunity.ProposedValue.Should().Be(newProposedValue);
        saleOpportunity.Notes.Should().Be(newNotes);
    }

    [Fact]
    public void EnsureCanBeEdited_WhenFinalized_ShouldThrowDomainException()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        saleOpportunity.Update(1L, 2L, SaleOpportunityStatus.Sold, 85000m, "Test");

        // Act
        Action act = () => saleOpportunity.EnsureCanBeEdited(1L, 2L, SaleOpportunityStatus.Sold);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*finalizada*");
    }

    [Fact]
    public void EnsureCanBeEdited_WhenChangingVehicleInNegotiation_ShouldThrowDomainException()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        saleOpportunity.Update(1L, 2L, SaleOpportunityStatus.InNegotiation, 85000m, "Test");

        // Act
        Action act = () => saleOpportunity.EnsureCanBeEdited(1L, 3L, SaleOpportunityStatus.InNegotiation);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*veículo*Negociação*");
    }

    [Fact]
    public void EnsureCanBeEdited_WhenChangingCustomerInNegotiation_ShouldThrowDomainException()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        saleOpportunity.Update(1L, 2L, SaleOpportunityStatus.InNegotiation, 85000m, "Test");

        // Act
        Action act = () => saleOpportunity.EnsureCanBeEdited(3L, 2L, SaleOpportunityStatus.InNegotiation);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*cliente*Negociação*");
    }

    [Fact]
    public void EnsureCanBeEdited_WhenChangingStatusFromInNegotiationToNewLead_ShouldThrowDomainException()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        saleOpportunity.Update(1L, 2L, SaleOpportunityStatus.InNegotiation, 85000m, "Test");

        // Act
        Action act = () => saleOpportunity.EnsureCanBeEdited(1L, 2L, SaleOpportunityStatus.NewLead);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Em negociação*Novo lead*");
    }

    [Fact]
    public void EnsureCanBeEdited_WhenChangingStatusFromProposalSentToNewLead_ShouldThrowDomainException()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        saleOpportunity.Update(1L, 2L, SaleOpportunityStatus.ProposalSent, 85000m, "Test");

        // Act
        Action act = () => saleOpportunity.EnsureCanBeEdited(1L, 2L, SaleOpportunityStatus.NewLead);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Proposta enviada*Novo lead*");
    }

    [Fact]
    public void EnsureCanBeEdited_WhenChangingStatusFromProposalSentToInNegotiation_ShouldThrowDomainException()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Test");
        saleOpportunity.Update(1L, 2L, SaleOpportunityStatus.ProposalSent, 85000m, "Test");

        // Act
        Action act = () => saleOpportunity.EnsureCanBeEdited(1L, 2L, SaleOpportunityStatus.InNegotiation);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*Proposta enviada*Em negociação*");
    }

    [Fact]
    public void EnsureCanBeEdited_WithValidChanges_ShouldNotThrowException()
    {
        // Arrange
        var saleOpportunity = SaleOpportunity.Create(1L, 2L, 85000m, "Test");

        // Act
        Action act = () => saleOpportunity.EnsureCanBeEdited(1L, 2L, SaleOpportunityStatus.InNegotiation);

        // Assert
        act.Should().NotThrow();
    }
}

