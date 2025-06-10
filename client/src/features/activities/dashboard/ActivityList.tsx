import { Box } from "@mui/material";
import ActivityCard from "./ActivityCard";

interface ActivityDashboardProps {
    activities: Activity[];
    selectActivity: (id: string) => void;
    deleteActivity: (id:string) => void;
}

export default function ActivityList({activities, selectActivity, deleteActivity}: ActivityDashboardProps) {
  return (
    <Box sx={{display: 'flex', flexDirection:'column', gap:3}}>
        {activities.map(activity => (
            <ActivityCard deleteActivity={deleteActivity} key={activity.id} activity={activity} selectActivity={selectActivity} />
        ))}
    </Box>
  )
}
