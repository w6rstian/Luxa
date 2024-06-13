using Luxa.Models;
using System.ComponentModel.DataAnnotations;

namespace Luxa.ViewModel
{
    public class FollowsChangeVM
    {
        public List<FollowModel> PendingFollowRequests { get; set; } = new List<FollowModel>();
    }
}