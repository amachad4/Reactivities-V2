import {
  Button,
  Card,
  CardActions,
  CardContent,
  CardMedia,
  Typography,
} from "@mui/material";
import { useActivites } from "../../../lib/types/hooks/useActivities";

interface ActivityDetailsProps {
  selectedActivity: Activity;
  cancelSelectActivity: () => void;
  openForm: (id: string) => void;
}

export default function ActivityDetail({
  selectedActivity,
  cancelSelectActivity,
  openForm,
}: ActivityDetailsProps) {
  const { activities } = useActivites();
  const activity = activities?.find(
    (activity) => activity.id === selectedActivity.id
  );

  if (!activity) return <Typography>Load...</Typography>;
  return (
    <Card sx={{ borderRadius: 3 }}>
      <CardMedia
        component="img"
        src={`/images/categoryImages/${activity.category}.jpg`}
      />
      <CardContent>
        <Typography variant="h5">{activity.title}</Typography>
        <Typography variant="subtitle1" fontWeight="light">
          {activity.title}
        </Typography>
        <Typography variant="body1">{activity.description}</Typography>
      </CardContent>
      <CardActions>
        <Button onClick={() => openForm(activity.id)} color="primary">
          Edit
        </Button>
        <Button onClick={cancelSelectActivity} color="inherit">
          Cancel
        </Button>
      </CardActions>
    </Card>
  );
}
