using JurayUniversal.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Persistence.EF.SQL
{
    public static class MisWizardSeeder
    {
        public static void SeedMisWizards(this ModelBuilder builder)
        {
            // Seed the initial default widgets
            builder.Entity<MisWidget>().HasData(
                new MisWidget { Id = 1, WidgetCode = "welcome_wizard", SortOrder = 1, ColSpan = 5 },
                new MisWidget { Id = 2, WidgetCode = "system_health_hub", SortOrder = 2, ColSpan = 12 },
                new MisWidget { Id = 3, WidgetCode = "applications_hub", SortOrder = 3, ColSpan = 12 },
                new MisWidget { Id = 5, WidgetCode = "recent_visited", SortOrder = 5, ColSpan = 7 }
            );

            // Seed default Wizard steps
            builder.Entity<MisWizardStep>().HasData(
                // Welcome Wizard (Main MIS Console)
                new MisWizardStep { Id = 1, FeatureName = "MIS", StepOrder = 1, TargetElement = "#widget-welcome_wizard", Title = "Welcome to MIS Console", Content = "This is your main dashboard where you manage all platform modules." },
                new MisWizardStep { Id = 2, FeatureName = "MIS", StepOrder = 2, TargetElement = "#addWidgetBtn", Title = "Add Custom Widgets", Content = "Click here to add new charts, lists, and metrics to your dashboard." },
                new MisWizardStep { Id = 3, FeatureName = "MIS", StepOrder = 3, TargetElement = ".btn-outline-secondary", Title = "Rearrange Layout", Content = "Toggle edit mode to drag, resize, and remove widgets." },

                // User Management Wizard
                new MisWizardStep { Id = 4, FeatureName = "MisUserManagement", StepOrder = 1, TargetElement = ".page-header", Title = "User Directory", Content = "Manage all your staff, members, and custom KPIs from this central directory." },
                new MisWizardStep { Id = 5, FeatureName = "MisUserManagement", StepOrder = 2, TargetElement = "#btn-add-user", Title = "Add New User", Content = "Click here to invite or manually add a new user to the platform." },
                new MisWizardStep { Id = 6, FeatureName = "MisUserManagement", StepOrder = 3, TargetElement = "#users-table", Title = "User List", Content = "View, filter, and manage permissions for existing users." },

                // Ticketing & Events Wizard
                new MisWizardStep { Id = 7, FeatureName = "MisTicketing", StepOrder = 1, TargetElement = ".page-header", Title = "Event Ticketing", Content = "Create and manage concerts, conferences, and paid events." },
                new MisWizardStep { Id = 8, FeatureName = "MisTicketing", StepOrder = 2, TargetElement = "#btn-create-event", Title = "Create Event", Content = "Setup a new event, configure ticket limits, and pricing." },

                // Financials & Dues Wizard
                new MisWizardStep { Id = 9, FeatureName = "MisDues", StepOrder = 1, TargetElement = ".page-header", Title = "Financials", Content = "Track subscriptions, recurring fees, and member dues." },
                new MisWizardStep { Id = 10, FeatureName = "MisDues", StepOrder = 2, TargetElement = "#btn-new-invoice", Title = "Generate Invoices", Content = "Send automated payment reminders to members." },

                // Committees & Groups Wizard
                new MisWizardStep { Id = 11, FeatureName = "MisCommittees", StepOrder = 1, TargetElement = ".page-header", Title = "Committees", Content = "Manage private workspaces and specialized teams." },
                new MisWizardStep { Id = 12, FeatureName = "MisCommittees", StepOrder = 2, TargetElement = "#btn-add-group", Title = "Create Workspace", Content = "Start a new collaboration group and invite members." },

                // Leaves & Excuses Wizard (Admin View)
                new MisWizardStep { Id = 13, FeatureName = "MisUserManagement_Leaves", StepOrder = 1, TargetElement = ".page-header", Title = "Leaves & Excuses", Content = "Review all leave requests and excuses submitted by platform users." },
                new MisWizardStep { Id = 14, FeatureName = "MisUserManagement_Leaves", StepOrder = 2, TargetElement = "#leaves-table", Title = "Request Queue", Content = "Click 'Review' on any pending request to approve or deny it with remarks." },

                // Leaves & Excuses Wizard (User View)
                new MisWizardStep { Id = 15, FeatureName = "Platform_Leaves", StepOrder = 1, TargetElement = "#btn-new-request", Title = "Submit a Request", Content = "Need time off or running late? Click here to submit a formal request." },
                new MisWizardStep { Id = 16, FeatureName = "Platform_Leaves", StepOrder = 2, TargetElement = "#my-requests-table", Title = "Track Status", Content = "Monitor the approval status of your submitted requests here." }
            );
        }
    }
}
